using FileManagement.Model;

namespace FileManagement.Models
{
    public class FileDetailsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public int FileTypeId { get; set; }
        public string FileType { get; set; }
        public string FileIcon { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string SizeString { get; set; }
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public int UserGroupId { get; set; }
        public UserGroupMappingModel userGroupMapping { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
