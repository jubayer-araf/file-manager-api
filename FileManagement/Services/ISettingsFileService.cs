using FileManagement.Entities;

namespace FileManagement.Services
{
    public interface ISettingsFileService
    {
        Task StoreFileAsync(IFormFile file, string filename);
        Task<byte[]> GetFileFromStorage(string fileId);
    }
}
