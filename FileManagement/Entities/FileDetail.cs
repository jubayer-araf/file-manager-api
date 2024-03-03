namespace FileManagement.Entities
{
    public class FileDetail : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public int FileTypeId { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string? FolderId { get; set; }
        public int UserGroupId { get; set; }
        public string? UserGroupName { get; set; }
        public string OwnerId { get; set; }

        public FileDetail() 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
