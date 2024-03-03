namespace FileManagement.Entities
{
    public class FileType : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int UserGroupId { get; set; }
        public string? UserGroupName { get; set; }
    }
}
