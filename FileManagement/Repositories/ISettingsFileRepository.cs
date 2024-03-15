using FileManagement.AppDbContext;
using FileManagement.Entities;

namespace FileManagement.Repositories
{
    public interface ISettingsFileRepository
    {
        Task<SettingsFile> GetSettingsFile(string fileId, string ownerId);
        Task<bool> FileNameExists(string fileName, string ownerId);
        Task<SettingsFile> AddSettingsFile(SettingsFile settingsFile);
        Task<SettingsFile> UpdateSettingsFile(SettingsFile settingsFile);

    }
}
