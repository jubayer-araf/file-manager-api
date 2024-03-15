using UserManagement.Entities;

namespace UserManagement.Repositories
{
    public interface IUserSettingsRepository
    {
        Task<UserSetting> GetUserSettingByIdAsync(string userId);
        Task<UserSetting> AddUserSettingAsync(UserSetting userSetting);
        UserSetting UpdateUserSetting(UserSetting userSetting);
        Task<bool> SaveChanges();
    }
}
