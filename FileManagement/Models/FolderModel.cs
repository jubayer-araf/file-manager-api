namespace FileManagement.Models
{
    public class FolderModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ParentFolderId { get; set; }
    }
}
