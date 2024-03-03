using FileManagement.Entities;

namespace FileManagement.Models
{
    public class FolderResModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FolderPath { get; set; }
        public int Size { get; set; }
        public string? ParentFolderId { get; set; }
        public int UserGroupId { get; set; }
        public string? UserGroupName { get; set; }
        public string OwnerId { get; set; }
        public List<FolderDetail>? ChildFolders { get; set; }
        public List<FileDetail>? FileItems { get; set; }
    }
}
