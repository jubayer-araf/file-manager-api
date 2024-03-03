using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Authentication;
using UserManagement.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("users")]
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userService.AllUserList();
        }

        [Authorize]
        [HttpGet]
        [Route("currentUser")]
        public async Task<ApplicationUser> GetCurrentUser()
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                return await _userService.GetCurrentuser(currentUserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Authorize]
        [HttpGet("searchUsers/{key}")]
        public async Task<IActionResult> SearchUsers(string key)
        {
            try
            {
                var searchedUserList = await _userService.SearchByUserNameKey(key);

                if (searchedUserList == null) return NotFound();

                return Ok(searchedUserList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
