using FileManagement.AppDbContext;
using FileManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Repositories
{
    public class SettingsFileRepository : ISettingsFileRepository
    {
        private readonly FileManagementDbContext _fileManagementDbContext;

        public SettingsFileRepository(FileManagementDbContext fileManagementDbContext)
        {
            _fileManagementDbContext = fileManagementDbContext;
        }
        public async Task<SettingsFile> GetSettingsFile(string fileId, string ownerId)
        {
            return await _fileManagementDbContext.SettingsFiles.FirstOrDefaultAsync(x => x.Id == fileId && x.OwnerId == ownerId && !x.isDeleted);
        }

        public async Task<bool> FileNameExists(string fileName, string ownerId)
        {
            return await _fileManagementDbContext.SettingsFiles
                .AnyAsync(b => b.Name == fileName && b.OwnerId == ownerId && !b.isDeleted);
        }

        public async Task<SettingsFile> AddSettingsFile(SettingsFile settingsFile)
        {
            await _fileManagementDbContext.SettingsFiles.AddAsync(settingsFile);
            await _fileManagementDbContext.SaveChangesAsync();
            return settingsFile;
        }

        public async Task<SettingsFile> UpdateSettingsFile(SettingsFile settingsFile)
        {
            _fileManagementDbContext.SettingsFiles.Update(settingsFile);
            await _fileManagementDbContext.SaveChangesAsync();
            return settingsFile;
        }
    }
}
