using Microsoft.EntityFrameworkCore;
using UserManagement.Authentication;
using UserManagement.Entities;

namespace UserManagement.Repositories
{
    public class UserGroupsRepository: IUserGroupsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserGroupsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<UserGroup>> GetAllUserGroup()
        {
            return await _applicationDbContext.UserGroups.ToListAsync();
        }

        public async Task<UserGroup> GetUserGroupById(int groupId)
        {
            return await _applicationDbContext.UserGroups
                .Where(b => b.Id == groupId && !b.isDeleted).FirstOrDefaultAsync();
        }
        public async Task<bool> UserGroupExists(string groupName)
        {
            return await _applicationDbContext.UserGroups
                .AnyAsync(b => b.Name == groupName && !b.isDeleted);
        }

        public async Task<UserGroup> AddUserGroup(UserGroup userGroup)
        {
            await _applicationDbContext.UserGroups.AddAsync(userGroup);
            return userGroup;
        }

        public UserGroup UpdateUserGroup(UserGroup userGroup)
        {
            _applicationDbContext.UserGroups.Update(userGroup);
            return userGroup;
        }
        public async Task<UserGroupMapping> AddUserGroupMapping(UserGroupMapping userGroupMapping)
        {
            await _applicationDbContext.UserGroupMappings.AddAsync(userGroupMapping);
            return userGroupMapping;
        }
        public UserGroupMapping UpdateUserGroupMapping(UserGroupMapping userGroupMapping)
        {
            _applicationDbContext.UserGroupMappings.Update(userGroupMapping);
            return userGroupMapping;
        }
        public async Task<IEnumerable<UserGroupMapping>> GetAllGroupMappingsAsync()
        {
            return await _applicationDbContext.UserGroupMappings.Where(
                x=>x.isDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<UserGroupMapping>> GetGroupMappingsForUserAsync(string userId)
        {
            return await _applicationDbContext.UserGroupMappings.Where(
                x => x.isDeleted == false && x.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<UserGroupMapping>> GetGroupMappingsForGroupAsync(int groupId)
        {
            return await _applicationDbContext.UserGroupMappings.Where(
                x => x.isDeleted == false && x.UserGroupId == groupId).ToListAsync();
        }
        public async Task<bool> SaveChanges()
        {
            return (await _applicationDbContext.SaveChangesAsync() > 0);
        }
    }
}
