using FileManagement.Entities;
using FileManagement.Helpers;
using FileManagement.Model;
using FileManagement.Models;
using FileManagement.Repositories;
using FileManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDetailsController : ControllerBase
    {
        private readonly ICustomAuthorizeService _customAuthorizeService;
        private readonly IUserManagementService _userManagementService;
        private readonly IFileDetailsRepository _fileDetailsRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileService _fileService;

        public FileDetailsController(ICustomAuthorizeService customAuthorizeService,
            IUserManagementService userManagementService,
            IFileDetailsRepository fileDetailsRepository,
            IFolderRepository folderRepository,
            IFileService fileService) 
        { 
            _customAuthorizeService = customAuthorizeService;
            _userManagementService = userManagementService;
            _fileDetailsRepository = fileDetailsRepository;
            _folderRepository = folderRepository;
            _fileService = fileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var fileDetails = await _fileDetailsRepository.GetFileDetails(id);
                    if (fileDetails != null)
                    {
                        var userGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, fileDetails.UserGroupId);

                        var hasAccess = userGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                        if (!hasAccess)
                        {
                            return NoContent();
                        }

                        var fileType = await _fileDetailsRepository.GetFileTypeByIdAsync(fileDetails.FileTypeId);
                        var parentFolder = await _folderRepository.GetFolderDetails(fileDetails.FolderId);

                        FileDetailsResponse fileDetailsResponse = new FileDetailsResponse
                        {
                            Id = fileDetails.Id,
                            Name = fileDetails.Name,
                            Description = fileDetails.Description,
                            FilePath = fileDetails.FilePath,
                            FileTypeId = fileDetails.FileTypeId,
                            FileType = fileType != null ? fileType.Name : "",
                            FileIcon = fileType != null ? fileType.Description : "",
                            Extension = fileDetails.Extension,
                            Size = fileDetails.Size,
                            SizeString = FileSizeFormatter.FormatSize(fileDetails.Size),
                            FolderId = fileDetails.FolderId,
                            FolderName = parentFolder != null ? parentFolder.Name : "",
                            UserGroupId = fileDetails.UserGroupId,
                            userGroupMapping = userGroupMapping,
                            OwnerId = fileDetails.OwnerId,
                            OwnerName = userGroupMapping.userGroupMappings.Where(x => x.UserId == fileDetails.OwnerId).FirstOrDefault().UserName,
                            UpdatedDate = fileDetails.UpdatedDate,
                            CreatedDate = fileDetails.CreatedDate,
                            CreatedBy = userGroupMapping.userGroupMappings.Where(x => x.UserId == fileDetails.CreatedByUserId).FirstOrDefault().UserName,
                            LastUpdatedBy = userGroupMapping.userGroupMappings.Where(x => x.UserId == fileDetails.LastUpdatedByUserId).FirstOrDefault().UserName,
                            isDeleted = fileDetails.isDeleted
                        };

                        return Ok(fileDetailsResponse);
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

        // POST api/<FileController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUploadModel fileUploadModel)
        {
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null && fileUploadModel.FormFile != null)
            {
                var currentFolder = await _folderRepository.GetFolderDetails(fileUploadModel.FolderId);
                if (currentFolder != null)
                {
                    var userGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, currentFolder.UserGroupId);

                    var hasAccess = userGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                    if (!hasAccess)
                    {
                        return BadRequest();
                    }

                    var fileName = fileUploadModel.FormFile.FileName;


                    var pathPrefix = await _folderRepository.GetPathPrefix(fileUploadModel.FolderId);
                    var filePath = "/" + pathPrefix + fileName;
                    var fileExtension = Path.GetExtension(fileName);

                    while (await _fileDetailsRepository.FileNameExists(filePath, applicationUser.Id))
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "_copy" + fileExtension;
                        filePath = "/" + pathPrefix + fileName;
                    }

                    var userGroup = await _userManagementService.AddUserGroup(applicationUser.AccessToken,
                            new UserGroupModel
                            {
                                Name = applicationUser.UserName + "-" + fileName,
                                Description = $"User group for {fileName} file of user {applicationUser.UserName}"
                            });
                    var response = await _userManagementService.AddToUserGroup(applicationUser.AccessToken, userGroup.Id);

                    FileDetail fileDetail = new FileDetail
                    {
                        Name = fileName,
                        Description = $"This file is uploaded with name : {fileName}",
                        Size = (int)fileUploadModel.FormFile.Length,
                        FilePath = "/" + pathPrefix + fileName,
                        FolderId = fileUploadModel.FolderId,
                        FileTypeId = 1,
                        Extension = fileExtension,
                        UserGroupId = userGroup.Id,
                        UserGroupName = userGroup.Name,
                        OwnerId = currentFolder.OwnerId,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        CreatedByUserId = applicationUser.Id,
                        LastUpdatedByUserId = applicationUser.Id,
                        isDeleted = false
                    };

                    var fileDetailRes = await _fileDetailsRepository.AddFile(fileDetail);
                    await _fileDetailsRepository.SaveChangesAsync();

                    await _fileService.StoreFileAsync(fileUploadModel.FormFile, fileDetailRes.Id);

                    var parentFolder = await _folderRepository.GetFolderDetails(fileUploadModel.FolderId);
                    parentFolder.Size++;
                    _folderRepository.UpdateFolder(parentFolder);
                    await _folderRepository.SaveChangesAsync();

                    return Ok(fileDetailRes);
                }
                return NoContent();
            }
            return Unauthorized();
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                var currentFileDetails = await _fileDetailsRepository.GetFileDetails(id);
                if (currentFileDetails != null)
                {
                    var userGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, currentFileDetails.UserGroupId);

                    var hasAccess = userGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                    if (hasAccess)
                    {
                        var fileBytes = await _fileService.GetFileFromStorage(currentFileDetails.Id);
                        if (fileBytes != null)
                        {
                            Stream stream = new MemoryStream(fileBytes);
                            return File(stream, "application/octet-stream", $"{currentFileDetails.Name}{currentFileDetails.Extension}");
                        }
                        return NotFound();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("filetype")]
        public async Task<IActionResult> FileType([FromBody] FileType fileType)
        {
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                if(fileType.Id > 0)
                {
                    var updatedFileType = _fileDetailsRepository.UpdateFileType(fileType);
                    await _fileDetailsRepository.SaveChangesAsync();
                    
                    return Ok(updatedFileType);
                }
                else
                {
                    var addFileType = await _fileDetailsRepository.AddFileType(fileType);
                    await _fileDetailsRepository.SaveChangesAsync();

                    return Ok(addFileType);
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("shared")]
        public async Task<IActionResult> SharedFiles()
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var userGroupLists = await _userManagementService.GetUserGroupByUserId(applicationUser.AccessToken);
                    if (userGroupLists != null)
                    {
                        List<FileDetail> files = new List<FileDetail>();
                        foreach(var userGroup in userGroupLists)
                        {
                            var fileDetail = await _fileDetailsRepository.GetFileDetailsByUserGroup(userGroup.Id, applicationUser.Id);
                            if (fileDetail != null) files.Add(fileDetail);
                        }
                        return Ok(files);
                    }
                    return NoContent();
                }

                return Unauthorized();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] FolderModel folderModel)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var currentFile = await _fileDetailsRepository.GetFileDetails(id, applicationUser.Id);
                    if (currentFile != null)
                    {
                        currentFile.Name = folderModel.Name;
                        currentFile.Description = folderModel.Description;
                        currentFile.UpdatedDate = DateTime.UtcNow;
                        currentFile.LastUpdatedByUserId = applicationUser.Id;

                        if (folderModel.ParentFolderId != null && currentFile.FolderId != null && !currentFile.FolderId.Equals(folderModel.ParentFolderId))
                        {
                            currentFile.FolderId = folderModel.ParentFolderId;
                        }
                        var pathPrefix = await _folderRepository.GetPathPrefix(folderModel.ParentFolderId);
                        var filePath = "/" + pathPrefix + folderModel.Name;

                        if (await _fileDetailsRepository.FileNameExists(filePath, currentFile.OwnerId)) return Conflict("File name already exists!");

                        currentFile.FilePath = filePath;


                        var updatedFolder = _fileDetailsRepository.UpdateFileDetails(currentFile);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var currentFile = await _fileDetailsRepository.GetFileDetails(id, applicationUser.Id);
                    if (currentFile != null)
                    {
                        currentFile.DeletedByUser = true;
                        currentFile.isDeleted = true;
                        currentFile.UpdatedDate = DateTime.Now;
                        var updatedFolder = _fileDetailsRepository.UpdateFileDetails(currentFile);
                        await _folderRepository.SaveChangesAsync();

                        await _userManagementService.DeleteGroupByUserGroupId(applicationUser.AccessToken, currentFile.UserGroupId);

                        if (currentFile.FolderId != null)
                        {
                            var parentFolder = await _folderRepository.GetFolderDetails(currentFile.FolderId);
                            parentFolder.Size--;
                            _folderRepository.UpdateFolder(parentFolder);
                            await _folderRepository.SaveChangesAsync();
                        }

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

    }
}
