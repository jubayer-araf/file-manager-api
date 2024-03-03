namespace FileManagement.Models
{
    public class FileUploadModel
    {
        public IFormFile FormFile { get; set; }
        public string FolderId { get; set; }
    }
}
