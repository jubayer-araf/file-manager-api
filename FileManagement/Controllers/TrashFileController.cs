using FileManagement.Entities;
using FileManagement.Models;
using FileManagement.Repositories;
using FileManagement.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrashFileController : ControllerBase
    {
        private readonly ICustomAuthorizeService _customAuthorizeService;
        private readonly ITrashRepository _trashRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IFileDetailsRepository _fileDetailsRepository;

        public TrashFileController(ICustomAuthorizeService customAuthorizeService
            , ITrashRepository trashRepository
            , IFolderRepository folderRepository
            , IUserManagementService userManagementService
            , IFileDetailsRepository fileDetailsRepository)
        {
            _customAuthorizeService = customAuthorizeService;
            _trashRepository = trashRepository;
            _folderRepository = folderRepository;
            _userManagementService = userManagementService;
            _fileDetailsRepository = fileDetailsRepository;
        }
        // GET: api/<TrashFileController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var deletedFolders = await _trashRepository.GetDeletedFolders(applicationUser.Id);
                    var deletedFiles = await _trashRepository.GetDeletedChildFileWithoutFolder(applicationUser.Id);
                    return Ok(new FolderResModel
                    {
                        ChildFolders = deletedFolders,
                        FileItems = deletedFiles
                    });
                }

                return Unauthorized();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<TrashFileController>
        [HttpGet("restorefile/{fileId}")]
        public async Task<IActionResult> RestoreFile(string fileId)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var deletedFile = await _trashRepository.GetFileDetails(fileId, applicationUser.Id);
                    if(deletedFile != null)
                    {
                        deletedFile.DeletedByUser = false;
                        deletedFile.isDeleted = false;
                        deletedFile.UpdatedDate = DateTime.Now;

                        if (deletedFile.FolderId != null)
                        {
                            var parentFolder = await _folderRepository.GetFolderDetails(deletedFile.FolderId);
                            if(parentFolder != null)
                            {
                                parentFolder.Size++;
                                _folderRepository.UpdateFolder(parentFolder);
                                await _folderRepository.SaveChangesAsync();
                            }
                            else
                            {
                                deletedFile.FolderId = "";
                            }

                        }

                        var updatedFolder = _fileDetailsRepository.UpdateFileDetails(deletedFile);
                        await _fileDetailsRepository.SaveChangesAsync();
                        await _userManagementService.RestoreGroupByUserGroupId(applicationUser.AccessToken, deletedFile.UserGroupId);

                        return Ok(deletedFile);
                    }
                    return NoContent();
                }

                return Unauthorized();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<TrashFileController>
        [HttpGet("restorefolder/{folderId}")]
        public async Task<IActionResult> RestoreFolder(string folderId)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var deletedFolder = await _trashRepository.GetFolderDetails(folderId, applicationUser.Id);
                    if (deletedFolder != null)
                    {
                        deletedFolder.DeletedByUser = false;
                        deletedFolder.isDeleted = false;
                        deletedFolder.UpdatedDate = DateTime.Now;

                        var updatedFolder = _folderRepository.UpdateFolder(deletedFolder);

                        var childFolders = await _trashRepository.GetChildFolders(deletedFolder.Id, applicationUser.Id);

                        if (childFolders != null)
                        {
                            await RestoreChilden(childFolders, applicationUser);
                        }


                        var childFiles = await _trashRepository.GetChildFiles(deletedFolder.Id, applicationUser.Id);
                        if (childFiles != null && childFiles.Count > 0)
                        {
                            foreach (var childFile in childFiles)
                            {
                                childFile.isDeleted = false;
                                childFile.DeletedByUser = false;
                                childFile.UpdatedDate = DateTime.Now;
                                _fileDetailsRepository.UpdateFileDetails(childFile);
                                await _userManagementService.RestoreGroupByUserGroupId(applicationUser.AccessToken, childFile.UserGroupId);
                            }
                        }

                        await _userManagementService.RestoreGroupByUserGroupId(applicationUser.AccessToken, deletedFolder.UserGroupId);

                        await _folderRepository.SaveChangesAsync();

                        return Ok(updatedFolder);
                    }
                    return NoContent();
                }

                return Unauthorized();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task RestoreChilden(List<FolderDetail> childFolders, ApplicationUser applicationUser)
        {
            if (childFolders == null || childFolders.Count <= 0) return;

            foreach (var childFolder in childFolders)
            {
                var nestedChildFiles = await _trashRepository.GetChildFiles(childFolder.Id, applicationUser.Id);
                if (nestedChildFiles != null && nestedChildFiles.Count > 0)
                {
                    foreach (var nestedChildFile in nestedChildFiles)
                    {
                        nestedChildFile.DeletedByUser = false;
                        nestedChildFile.isDeleted = false;
                        nestedChildFile.UpdatedDate = DateTime.Now;
                        _fileDetailsRepository.UpdateFileDetails(nestedChildFile);
                        await _userManagementService.RestoreGroupByUserGroupId(applicationUser.AccessToken, nestedChildFile.UserGroupId);
                    }
                }

                childFolder.isDeleted = false;
                childFolder.DeletedByUser = false;
                childFolder.UpdatedDate = DateTime.Now;
                _folderRepository.UpdateFolder(childFolder);

                await _userManagementService.RestoreGroupByUserGroupId(applicationUser.AccessToken, childFolder.UserGroupId);


                var nestedchildFolders = await _trashRepository.GetChildFolders(childFolder.Id, applicationUser.Id);
                await RestoreChilden(nestedchildFolders, applicationUser);
            }
        }
    }
}
