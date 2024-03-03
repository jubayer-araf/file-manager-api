using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using UserManagement.Authentication;
using UserManagement.Entities;
using UserManagement.Model;
using UserManagement.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupMappingController : ControllerBase
    {
        private readonly IUserGroupsRepository _userGroupsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserGroupMappingController(IUserGroupsRepository userGroupsRepository, UserManager<ApplicationUser> userManager) 
        {
            _userGroupsRepository = userGroupsRepository;
            _userManager = userManager;
        }

        // GET: api/<UserGroupMappingController>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IEnumerable<UserGroupMappingModel>> Get()
        {
            var userGroups = await _userGroupsRepository.GetAllUserGroup();

            List<UserGroupMappingModel> userGroupMappingModels = new List<UserGroupMappingModel>();
            
            foreach (var userGroup in userGroups)
            {
                if (userGroup != null)
                {
                    var userGroupMapping = await _userGroupsRepository.GetGroupMappingsForGroupAsync(userGroup.Id);
                    UserGroupMappingModel userGroupMappingModel = new UserGroupMappingModel {
                        userGroup = userGroup, 
                        userGroupMappings = userGroupMapping };
                    userGroupMappingModels.Add(userGroupMappingModel);
                }

            }


            return userGroupMappingModels;
        }

        // GET api/<UserGroupMappingController>/5
        [Authorize]
        [HttpGet("{groupId}")]
        public async Task<UserGroupMappingModel> Get(int groupId)
        {
            var userGroup = await _userGroupsRepository.GetUserGroupById(groupId);
            if (userGroup != null)
            {
                var userGroupMapping = await _userGroupsRepository.GetGroupMappingsForGroupAsync(userGroup.Id);
                return new UserGroupMappingModel
                {
                    userGroup = userGroup,
                    userGroupMappings = userGroupMapping
                };
            }
            return null;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getByUserId/{userId}")]
        public async Task<IEnumerable<UserGroup>> GetByUserId(string userId)
        {
            return await getUserGroupsById(userId);
        }

        [Authorize]
        [HttpGet]
        [Route("currentUserGroup")]
        public async Task<IEnumerable<UserGroup>> GetByUserId()
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                return await getUserGroupsById(currentUser.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        // POST api/<UserGroupMappingController>
        [Authorize]
        [HttpPost]
        public async Task<Response> Post([FromBody] int userGroupId)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                var userGroup = await _userGroupsRepository.GetUserGroupById(userGroupId);

                UserGroupMapping userGroupMapping = new UserGroupMapping
                {
                    CreatedByUserId = Convert.ToString(currentUser.Id),
                    LastUpdatedByUserId = Convert.ToString(currentUser.Id),
                    UserGroupId = userGroupId,
                    UserId = currentUser.Id,
                    UserName = currentUser.UserName,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var userGroupRes = await _userGroupsRepository.AddUserGroupMapping(userGroupMapping);
                await _userGroupsRepository.SaveChanges();
                return new Response
                {
                    Message = $"User is added to {userGroup.Name} user group. Today is {userGroupMapping.CreatedDate}.",
                    Status = "Success"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Message = ex.Message,
                    Status = ex.StackTrace
                };
            }
        }

        [Authorize]
        [HttpPost]
        [Route("addUserToGroup")]
        public async Task<Response> ManualAdd([FromBody] UserGroupMappingRequestModel userGroupMappingRequestModel)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                var userGroup = await _userGroupsRepository.GetUserGroupById(userGroupMappingRequestModel.UserGroupId);

                UserGroupMapping userGroupMapping = new UserGroupMapping
                {
                    CreatedByUserId = Convert.ToString(currentUser.Id),
                    LastUpdatedByUserId = Convert.ToString(currentUser.Id),
                    UserGroupId = userGroupMappingRequestModel.UserGroupId,
                    UserId = userGroupMappingRequestModel.UserId,
                    UserName = userGroupMappingRequestModel.UserName,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var userGroupRes = await _userGroupsRepository.AddUserGroupMapping(userGroupMapping);
                await _userGroupsRepository.SaveChanges();
                return new Response
                {
                    Message = $"User is added to {userGroup.Name} user group. Today is {userGroupMapping.CreatedDate}.",
                    Status = "Success"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Message = ex.Message,
                    Status = ex.StackTrace
                };
            }
        }

        private async Task<IEnumerable<UserGroup>> getUserGroupsById(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<UserGroup> userGroups = new List<UserGroup>();
                if (user == null)
                {
                    return null;
                }
                var userGroupMappings = await _userGroupsRepository.GetGroupMappingsForUserAsync(user.Id);

                foreach (var userGroupMapping in userGroupMappings)
                {
                    userGroups.Add(await _userGroupsRepository.GetUserGroupById(userGroupMapping.UserGroupId));
                }

                return userGroups;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
