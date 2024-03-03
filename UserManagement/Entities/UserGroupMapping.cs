namespace UserManagement.Entities
{
    public class UserGroupMapping : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int UserGroupId { get; set; }
    }
}
