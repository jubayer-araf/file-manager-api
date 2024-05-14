using FileManagement.Entities;
using FileManagement.Model;
using FileManagement.Models;
using FileManagement.Repositories;
using FileManagement.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly ICustomAuthorizeService _customAuthorizeService;
        private readonly IUserManagementService _userManagementService;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileDetailsRepository _fileDetailsRepository;
        private readonly IFileService _fileService;

        public FolderController(ICustomAuthorizeService customAuthorizeService,
            IUserManagementService userManagementService,
            IFolderRepository folderRepository,
            IFileDetailsRepository fileDetailsRepository,
            IFileService fileService)
        {
            _customAuthorizeService = customAuthorizeService;
            _userManagementService = userManagementService;
            _folderRepository = folderRepository;
            _fileDetailsRepository = fileDetailsRepository;
            _fileService = fileService;
        }
        // GET: api/<FolderController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            FolderResModel folderResModel = new FolderResModel();
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                var currentFolder = await _folderRepository.GetFolderDetails(id, applicationUser.Id);
                if (currentFolder != null)
                {
                    folderResModel.Id = currentFolder.Id;
                    folderResModel.OwnerId = currentFolder.OwnerId;
                    folderResModel.Size = currentFolder.Size;
                    folderResModel.ParentFolderId = currentFolder.ParentFolderId;
                    folderResModel.Name = currentFolder.Name;
                    folderResModel.Description = currentFolder.Description;
                    folderResModel.FolderPath = currentFolder.FolderPath;
                    folderResModel.UserGroupId = currentFolder.UserGroupId;
                    folderResModel.UserGroupName = currentFolder.UserGroupName;

                    var childFolders = await _folderRepository.GetChildFolders(id, applicationUser.Id);
                    if (childFolders != null)
                    {
                        folderResModel.ChildFolders = childFolders;
                    }
                    var fileItems = await _fileDetailsRepository.GetChildFiles(id, applicationUser.Id);
                    if (fileItems != null)
                    {
                        folderResModel.FileItems = fileItems;
                    }
                    return Ok(folderResModel);
                }
                return NotFound();
            }
            return Unauthorized();
        }
        // GET api/<FolderController>/5
        [HttpGet("shared/{id}")]
        public async Task<IActionResult> GetShared(string id)
        {
            FolderResModel folderResModel = new FolderResModel();
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                var currentFolder = await _folderRepository.GetFolderDetails(id);
                if (currentFolder != null)
                {
                    var userGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, currentFolder.UserGroupId);

                    var hasAccess = userGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                    if (hasAccess)
                    {
                        folderResModel.Id = currentFolder.Id;
                        folderResModel.OwnerId = currentFolder.OwnerId;
                        folderResModel.Size = currentFolder.Size;
                        folderResModel.ParentFolderId = currentFolder.ParentFolderId;
                        folderResModel.Name = currentFolder.Name;
                        folderResModel.Description = currentFolder.Description;
                        folderResModel.FolderPath = currentFolder.FolderPath;
                        folderResModel.UserGroupId = currentFolder.UserGroupId;
                        folderResModel.UserGroupName = currentFolder.UserGroupName;

                        var childFolders = await _folderRepository.GetChildFolders(id);
                        if (childFolders != null)
                        {
                            folderResModel.ChildFolders = new List<FolderDetail>();
                            foreach (var childFolder in childFolders)
                            {
                                var childUserGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, childFolder.UserGroupId);

                                var childHasAccess = childUserGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                                if (childHasAccess)
                                {
                                    folderResModel.ChildFolders.Add(childFolder);
                                }
                            }
                        }

                        var fileItems = await _fileDetailsRepository.GetChildFiles(id);
                        if (fileItems != null)
                        {
                            folderResModel.FileItems = new List<FileDetail>();
                            foreach (var fileItem in fileItems)
                            {
                                var childUserGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, fileItem.UserGroupId);

                                var childHasAccess = childUserGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                                if (childHasAccess)
                                {
                                    folderResModel.FileItems.Add(fileItem);
                                }
                            }
                        }

                        return Ok(folderResModel);
                    }

                    return BadRequest();
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("root")]
        public async Task<IActionResult> GetRootFolders()
        {
            FolderResModel folderResModel = new FolderResModel();
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                var childFolders = await _folderRepository.GetRootFolders(applicationUser.Id);
                if (childFolders != null)
                {
                    folderResModel.ChildFolders = childFolders;
                }
                var fileItems = await _fileDetailsRepository.GetrootFiles(applicationUser.Id);
                if (fileItems != null)
                {
                    folderResModel.FileItems = fileItems;
                }
                return Ok(folderResModel);
            }
            return Unauthorized();
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFolder(string id)
        {
            FolderResModel folderResModel = new FolderResModel();
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                var currentFolder = await _folderRepository.GetFolderDetails(id);
                if (currentFolder != null)
                {
                    var userGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, currentFolder.UserGroupId);

                    var hasAccess = userGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                    if (hasAccess)
                    {
                        folderResModel.Id = currentFolder.Id;
                        folderResModel.OwnerId = currentFolder.OwnerId;
                        folderResModel.Size = currentFolder.Size;
                        folderResModel.ParentFolderId = currentFolder.ParentFolderId;
                        folderResModel.Name = currentFolder.Name;
                        folderResModel.Description = currentFolder.Description;
                        folderResModel.FolderPath = currentFolder.FolderPath;
                        folderResModel.UserGroupId = currentFolder.UserGroupId;
                        folderResModel.UserGroupName = currentFolder.UserGroupName;

                        var childFolders = await _folderRepository.GetChildFolders(id);
                        if (childFolders != null)
                        {
                            folderResModel.ChildFolders = new List<FolderDetail>();
                            foreach (var childFolder in childFolders)
                            {
                                var childUserGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, childFolder.UserGroupId);

                                var childHasAccess = childUserGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                                if (childHasAccess)
                                {
                                    folderResModel.ChildFolders.Add(childFolder);
                                }
                            }
                        }

                        var fileItems = await _fileDetailsRepository.GetChildFiles(id);
                        if (fileItems != null)
                        {
                            folderResModel.FileItems = new List<FileDetail>();
                            foreach (var fileItem in fileItems)
                            {
                                var childUserGroupMapping = await _userManagementService.GetUserGroupById(applicationUser.AccessToken, fileItem.UserGroupId);

                                var childHasAccess = childUserGroupMapping.userGroupMappings.Where(x => x.UserId == applicationUser.Id).Any();
                                if (childHasAccess)
                                {
                                    folderResModel.FileItems.Add(fileItem);
                                }
                            }
                        }

                        var fileBytes = await _fileService.ZipMultipleFileFromStorage(folderResModel.FileItems);
                        if (fileBytes != null)
                        {
                            return File(fileBytes, "application/zip", $"{folderResModel.Name}{DateTime.Now}.zip");
                        }

                        return Ok(folderResModel);
                    }

                    return BadRequest();
                }
                return NotFound();
            }
            return Unauthorized();
        }

        // POST api/<FolderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FolderModel folderModel)
        {
            ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
            if (applicationUser != null)
            {
                var userGroup = await _userManagementService.AddUserGroup(applicationUser.AccessToken, 
                    new UserGroupModel { Name = applicationUser.UserName + "-" + folderModel.Name, 
                        Description = $"User group for {folderModel.Name} folder of user {applicationUser.UserName}" });
                var response = await _userManagementService.AddToUserGroup(applicationUser.AccessToken, userGroup.Id);
                if(folderModel.ParentFolderId == "root")
                {
                    folderModel.ParentFolderId = "";
                }
                var pathPrefix = await _folderRepository.GetPathPrefix(folderModel.ParentFolderId);

                FolderDetail folderDetail = new FolderDetail
                {
                    Name = folderModel.Name,
                    Description = folderModel.Description,
                    Size = 0,
                    FolderPath = "/" + pathPrefix + folderModel.Name,
                    ParentFolderId = folderModel.ParentFolderId,
                    UserGroupId = userGroup.Id,
                    UserGroupName = userGroup.Name,
                    OwnerId = applicationUser.Id,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = applicationUser.Id,
                    LastUpdatedByUserId = applicationUser.Id,
                    isDeleted = false
                };

                var folderDetailRes = await _folderRepository.AddFolder(folderDetail);
                await _folderRepository.SaveChangesAsync();
                return Ok(folderDetailRes);
            }
            return Unauthorized();
        }

        // PUT api/<FolderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] FolderModel folderModel)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var currentFolder = await _folderRepository.GetFolderDetails(id);
                    if (currentFolder != null)
                    {
                        currentFolder.Name = folderModel.Name;
                        currentFolder.Description = folderModel.Description;
                        currentFolder.UpdatedDate = DateTime.UtcNow;
                        currentFolder.LastUpdatedByUserId = applicationUser.Id;
                        if (folderModel.ParentFolderId != null && currentFolder.ParentFolderId!= null && !currentFolder.ParentFolderId.Equals(folderModel.ParentFolderId))
                        {
                            currentFolder.ParentFolderId = folderModel.ParentFolderId;
                        }

                        var pathPrefix = await _folderRepository.GetPathPrefix(folderModel.ParentFolderId);
                        var folderPath = "/" + pathPrefix + folderModel.Name;
                        

                        if (await _folderRepository.FolderNameExists(folderPath, currentFolder.OwnerId)) return Conflict("Folder name already exists!");

                        currentFolder.FolderPath = folderPath;

                        var updatedFolder = _folderRepository.UpdateFolder(currentFolder);
                        await _folderRepository.SaveChangesAsync();

                        return Ok(updatedFolder);

                    }
                    return NoContent();
                }

                return Unauthorized();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<FolderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var currentFolder = await _folderRepository.GetFolderDetails(id);
                    if (currentFolder != null)
                    {
                        currentFolder.isDeleted = true;
                        var updatedFolder = _folderRepository.UpdateFolder(currentFolder);
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
    }
}
