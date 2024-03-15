using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Authentication;
using UserManagement.Entities;
using UserManagement.Model;
using UserManagement.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private readonly IUserGroupsRepository _userGroupsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserGroupsController(IUserGroupsRepository userGroupsRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userGroupsRepository = userGroupsRepository;

        }




        // GET: api/<UserGroupsController>
        [HttpGet]
        public async Task<IEnumerable<UserGroup>> Get()
        {
            return await _userGroupsRepository.GetAllUserGroup();
        }

        // GET api/<UserGroupsController>/5
        [HttpGet("{id}")]
        public async Task<UserGroup> Get(int id)
        {
            return await _userGroupsRepository.GetUserGroupById(id);
        }

        // POST api/<UserGroupsController>
        [Authorize]
        [HttpPost]
        public async Task<UserGroup> Post([FromBody] UserGroupModel userGroupModel)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userManager.FindByNameAsync(currentUserName);


                UserGroup userGroup = new UserGroup
                {
                    CreatedByUserId = Convert.ToString(currentUser.Id),
                    LastUpdatedByUserId = Convert.ToString(currentUser.Id),
                    Name = userGroupModel.Name,
                    Description = userGroupModel.Description,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var userGroupRes = await _userGroupsRepository.AddUserGroup(userGroup);
                await _userGroupsRepository.SaveChanges();
                return userGroupRes;
                //return new Response
                //{
                //    Message = $"New user group created with name {userGroup.Name}. Today is {userGroup.CreatedDate}.",
                //    Status = "Success"
                //};
            }
            catch (Exception ex)
            {
                throw ex;
                //return new Response
                //{
                //    Message = ex.Message,
                //    Status = ex.StackTrace
                //};
            }
            
        }

        // PUT api/<UserGroupsController>/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<Response> Put(int id, [FromBody] UserGroup userGroup)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userManager.FindByNameAsync(currentUserName);

                if (userGroup != null)
                {
                    userGroup.LastUpdatedByUserId = Convert.ToString(currentUser.Id);
                    userGroup.UpdatedDate = DateTime.Now;
                    var userGroupRes =  _userGroupsRepository.UpdateUserGroup(userGroup);
                    await _userGroupsRepository.SaveChanges();

                    return new Response
                    {
                        Message = $"User group updated with name {userGroup.Name}. Today is {userGroup.UpdatedDate}.",
                        Status = "Success"
                    };
                }
                else
                {
                    return new Response
                    {
                        Message = $"Couldn't upadate user group",
                        Status = "UnSuccessful"
                    };
                }



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

        // DELETE api/<UserGroupsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                
                await _userGroupsRepository.DeleteUserGroupMappingsByGroupId(id);
                await _userGroupsRepository.DeleteUserGroupId(id);
                await _userGroupsRepository.SaveChanges();

                return Ok("Curresponding user group mapping and user group is removed!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UserGroupsController>/5
        [Authorize]
        [HttpDelete("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {

                await _userGroupsRepository.RestoreUserGroupMappingsByGroupId(id);
                await _userGroupsRepository.RestoreUserGroupId(id);
                await _userGroupsRepository.SaveChanges();

                return Ok("Curresponding user group mapping and user group is restored!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
