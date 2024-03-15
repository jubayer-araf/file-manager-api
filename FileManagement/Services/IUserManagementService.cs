using FileManagement.Model;
using FileManagement.Models;

namespace FileManagement.Services
{
    public interface IUserManagementService
    {
        Task<ApplicationUser> GetCurrentUser(string token);
        Task<UserGroup> AddUserGroup(string token, UserGroupModel userGroupModel);
        Task<Response> AddToUserGroup(string token, int userGroupId);
        Task<UserGroupMappingModel> GetUserGroupById(string token, int userGroupId);
        Task<IEnumerable<UserGroup>> GetUserGroupByUserId(string token);
        Task<string> DeleteGroupByUserGroupId(string token, int userGroupId);
        Task<string> RestoreGroupByUserGroupId(string token, int userGroupId);
    }
}
