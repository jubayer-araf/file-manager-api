using FileManagement.Entities;
using FileManagement.Models;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace FileManagement.Services
{
    public class FileService : IFileService
    {
        private readonly IOptions<ConnectionStrings> _connectionStrings;
        private string uploads;

        public FileService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings;
            uploads = Path.Combine(_connectionStrings.Value.StorageString);
        }

        public async Task StoreFileAsync(IFormFile file, string filename)
        {
            //string path = Convert.ToString(x);
            //using (StreamWriter file =
            //    new StreamWriter(x, false))
            //{
            //    file.WriteLine(" XP : 0");
            //}

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

        public async Task<byte[]> ZipMultipleFileFromStorage(List<FileDetail> fileDetails)
        {
            if (fileDetails != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in fileDetails)
                        {
                            string filePath = Path.Combine(uploads, file.Id);
                            if (File.Exists(filePath))
                            {
                                var fileBytes = await File.ReadAllBytesAsync(filePath);
                                var entry = zip.CreateEntry($"{file.Name}{file.Extension}");
                                using (var fileStream = new MemoryStream(fileBytes))
                                using (var entryStream = entry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }
                    return ms.ToArray();
                }
            }


            return null;
        }
    }
}
