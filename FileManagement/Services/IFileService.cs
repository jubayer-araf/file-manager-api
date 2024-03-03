using FileManagement.Entities;

namespace FileManagement.Services
{
    public interface IFileService
    {
        Task StoreFileAsync(IFormFile file, string filename);
        Task<byte[]> GetFileFromStorage(string fileId);
        Task<byte[]> ZipMultipleFileFromStorage(List<FileDetail> fileDetails);
    }
}
