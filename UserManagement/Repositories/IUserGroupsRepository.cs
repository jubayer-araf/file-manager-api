using UserManagement.Entities;

namespace UserManagement.Repositories
{
    public interface IUserGroupsRepository
    {
        Task<IEnumerable<UserGroup>> GetAllUserGroup();
        Task<UserGroup> GetUserGroupById(int groupId);
        Task<bool> UserGroupExists(string groupName);
        Task<UserGroup> AddUserGroup(UserGroup userGroup);
        UserGroup UpdateUserGroup(UserGroup userGroup);
        Task<UserGroupMapping> AddUserGroupMapping(UserGroupMapping userGroupMapping);
        UserGroupMapping UpdateUserGroupMapping(UserGroupMapping userGroupMapping);
        Task<IEnumerable<UserGroupMapping>> GetAllGroupMappingsAsync();
        Task<IEnumerable<UserGroupMapping>> GetGroupMappingsForUserAsync(string userId);
        Task<IEnumerable<UserGroupMapping>> GetGroupMappingsForGroupAsync(int groupId);
        Task<bool> SaveChanges();
    }
}
