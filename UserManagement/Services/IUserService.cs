using UserManagement.Authentication;

namespace UserManagement.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentuser(string currentUserName);
        Task<IList<ApplicationUser>> SearchByUserNameKey(string usernameKey);
        Task<IList<ApplicationUser>> AllUserList();
    }
}
