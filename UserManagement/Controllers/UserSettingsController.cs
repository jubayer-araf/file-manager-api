using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Entities;
using UserManagement.Model;
using UserManagement.Repositories;
using UserManagement.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IUserService _userService;

        public UserSettingsController(IUserSettingsRepository userSettingsRepository, IUserService userService)
        {
            _userSettingsRepository = userSettingsRepository;
            _userService = userService;
        }

        // GET api/<UserSettingsController>/5
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userService.GetCurrentuser(currentUserName);
                return Ok(await _userSettingsRepository.GetUserSettingByIdAsync(currentUser.Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<UserSettingsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserSettingModel userSettingModel)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userService.GetCurrentuser(currentUserName);

                UserSetting userSetting = new UserSetting
                {
                    UserId = currentUser.Id,
                    UserName = currentUser.UserName,
                    FirstName = userSettingModel.FirstName,
                    LastName = userSettingModel.LastName,
                    Email = currentUser.Email,
                    AllocatedSpace = userSettingModel.AllocatedSpace,
                    DateFormat = userSettingModel.DateFormat,
                    TimeFormat = userSettingModel.TimeFormat,
                    IsDarkMode = userSettingModel.IsDarkMode,
                    UserLogoId = userSettingModel.UserLogoId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUserId = currentUser.Id,
                    LastUpdatedByUserId = currentUser.Id,
                    isDeleted = false
                };

                var userSettingAdd = await _userSettingsRepository.AddUserSettingAsync(userSetting);
                await _userSettingsRepository.SaveChanges();

                return Ok(userSettingAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserSettingsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserSettingModel userSettingModel)
        {
            try
            {
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _userService.GetCurrentuser(currentUserName);
                var userSetting = await _userSettingsRepository.GetUserSettingByIdAsync(currentUser.Id);

                userSetting.FirstName = userSettingModel.FirstName;
                userSetting.LastName = userSettingModel.LastName;
                userSetting.Email =  userSettingModel.Email;
                userSetting.DateFormat = userSettingModel.DateFormat;
                userSetting.TimeFormat = userSettingModel.TimeFormat;
                userSetting.IsDarkMode = userSettingModel.IsDarkMode;
                userSetting.UserLogoId = userSettingModel.UserLogoId;
                userSetting.UpdatedDate = DateTime.Now;
                userSetting.LastUpdatedByUserId = currentUser.Id;

                var userSettingUpdate = _userSettingsRepository.UpdateUserSetting(userSetting);
                await _userSettingsRepository.SaveChanges();

                return Ok(userSettingUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
