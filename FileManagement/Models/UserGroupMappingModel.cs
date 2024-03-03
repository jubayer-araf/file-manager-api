
namespace FileManagement.Model
{
    public class UserGroupMappingModel
    {
        public UserGroup userGroup { get; set; }
        public IEnumerable<UserGroupMapping> userGroupMappings { get; set; }
    }
}
