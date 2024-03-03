using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Authentication;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetCurrentuser(string currentUserName)
        {
            return await _userManager.FindByNameAsync(currentUserName);
        }

        public async Task<IList<ApplicationUser>> SearchByUserNameKey(string usernameKey)
        {
            var userList = await _userManager.Users.Where(x=>x.UserName.StartsWith(usernameKey)).ToListAsync();
            return userList;
        }

        public async Task<IList<ApplicationUser>> AllUserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            return userList;
        }

    }
}
