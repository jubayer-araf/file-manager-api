namespace UserManagement.Entities
{
    public class BaseEntity
    {
        public bool isDeleted { get; set; }
        public string CreatedByUserId { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
