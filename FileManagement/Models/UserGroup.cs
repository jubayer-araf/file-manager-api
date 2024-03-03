namespace FileManagement.Model
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedByUserId { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
