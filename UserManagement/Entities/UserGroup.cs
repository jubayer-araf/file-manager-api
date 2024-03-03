namespace UserManagement.Entities
{
    public class UserGroup : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
