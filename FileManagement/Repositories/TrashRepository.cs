using FileManagement.AppDbContext;
using FileManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Repositories
{
    public class TrashRepository : ITrashRepository
    {
        private readonly FileManagementDbContext _fileManagementDbContext;

        public TrashRepository(FileManagementDbContext fileManagementDbContext)
        {
            _fileManagementDbContext = fileManagementDbContext;
        }

        public async Task<FileDetail> GetFileDetails(string fileId, string userId)
        {
            return await _fileManagementDbContext.FileDetails.FirstOrDefaultAsync(x =>
            x.Id == fileId &&
            x.OwnerId == userId &&
            x.isDeleted);
        }
        public async Task<FolderDetail> GetFolderDetails(string folderId, string userId)
        {
            return await _fileManagementDbContext.FolderDetails.FirstOrDefaultAsync(x => 
            x.Id == folderId && 
            x.OwnerId == userId &&
            x.isDeleted);
        }
        public async Task<List<FolderDetail>> GetChildFolders(string folderId, string userId)
        {
            return await _fileManagementDbContext.FolderDetails
                .Where(x => x.ParentFolderId == folderId && x.OwnerId == userId &&
            x.isDeleted).ToListAsync();
        }

        public async Task<List<FileDetail>> GetChildFiles(string folderId, string userId)
        {
            return await _fileManagementDbContext.FileDetails.Where(x => x.FolderId == folderId && x.OwnerId == userId && x.isDeleted).ToListAsync();
        }

        public async Task<List<FolderDetail>> GetRootFolders(string userId)
        {
            return await _fileManagementDbContext.FolderDetails.Where(x => x.ParentFolderId == "" && x.OwnerId == userId &&
            x.isDeleted).ToListAsync();
        }

        public async Task<List<FileDetail>> GetDeletedChildFileWithoutFolder(string userId)
        {
            return await _fileManagementDbContext.FileDetails.Where(x=>x.OwnerId == userId && x.isDeleted && x.DeletedByUser).ToListAsync();
        }

        public async Task<List<FolderDetail>> GetDeletedFolders(string userId)
        {
            return await _fileManagementDbContext.FolderDetails.Where(x => x.OwnerId == userId && x.isDeleted && x.DeletedByUser).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _fileManagementDbContext.SaveChangesAsync() > 0);
        }
    }
}
