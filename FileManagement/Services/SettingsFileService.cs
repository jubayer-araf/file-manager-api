using FileManagement.Entities;
using FileManagement.Models;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace FileManagement.Services
{
    public class SettingsFileService : ISettingsFileService
    {
        private readonly IOptions<ConnectionStrings> _connectionStrings;
        private string uploads;

        public SettingsFileService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings;
            uploads = Path.Combine(_connectionStrings.Value.StorageString);
        }

        public async Task StoreFileAsync(IFormFile file, string filename)
        {
            if (file.Length > 0)
            {
                string filePath = Path.Combine(uploads, filename);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public async Task<byte[]> GetFileFromStorage(string fileId)
        {

            string filePath = Path.Combine(uploads, fileId);
            if (File.Exists(filePath))
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            return null;
        }

    }
}
