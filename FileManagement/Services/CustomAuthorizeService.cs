using FileManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileManagement.Services
{
    public class CustomAuthorizeService : ICustomAuthorizeService
    {
        private readonly IUserManagementService _userManagementService;

        public CustomAuthorizeService(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        public async Task<ApplicationUser> GetUserAsync(ControllerContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization") &&
                context.HttpContext.Request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = context.HttpContext.Request.Headers["Authorization"][0].Substring("Bearer ".Length); ;
                var currentUser = await _userManagementService.GetCurrentUser(token);
                if(currentUser!=null)
                {
                    currentUser.AccessToken = token;
                }
                return currentUser;
            }
            return null;
        }
    }
}
