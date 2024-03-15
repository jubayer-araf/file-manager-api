using Microsoft.EntityFrameworkCore;
using UserManagement.Authentication;
using UserManagement.Entities;

namespace UserManagement.Repositories
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserSettingsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<UserSetting> GetUserSettingByIdAsync(string userId)
        {
            return await _applicationDbContext.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<UserSetting> AddUserSettingAsync(UserSetting userSetting)
        {
            await _applicationDbContext.UserSettings.AddAsync(userSetting);
            return userSetting;
        }

        public UserSetting UpdateUserSetting(UserSetting userSetting)
        {
            _applicationDbContext.UserSettings.Update(userSetting);
            return userSetting;
        }
        public async Task<bool> SaveChanges()
        {
            return (await _applicationDbContext.SaveChangesAsync() > 0);
        }
    }
}
