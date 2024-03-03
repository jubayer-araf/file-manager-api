using FileManagement.AppDbContext;
using FileManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly FileManagementDbContext _fileManagementDbContext;

        public FolderRepository(FileManagementDbContext fileManagementDbContext)
        {
            _fileManagementDbContext = fileManagementDbContext;
        }
        public async Task<FolderDetail> GetFolderDetails(string folderId, string userId)
        {
            return await _fileManagementDbContext.FolderDetails.FirstOrDefaultAsync(x => 
            x.Id == folderId && 
            x.OwnerId == userId &&
            !x.isDeleted);
        }
        public async Task<List<FolderDetail>> GetChildFolders(string folderId, string userId)
        {
            return await _fileManagementDbContext.FolderDetails
                .Where(x => x.ParentFolderId == folderId && x.OwnerId == userId &&
            !x.isDeleted).ToListAsync();
        }

        public async Task<FolderDetail> GetFolderDetails(string folderId)
        {
            return await _fileManagementDbContext.FolderDetails.FirstOrDefaultAsync(x => x.Id == folderId &&
            !x.isDeleted);
        }
        public async Task<List<FolderDetail>> GetChildFolders(string folderId)
        {
            return await _fileManagementDbContext.FolderDetails.Where(x => x.ParentFolderId == folderId &&
            !x.isDeleted).ToListAsync();
        }

        public async Task<List<FolderDetail>> GetRootFolders(string userId)
        {
            return await _fileManagementDbContext.FolderDetails.Where(x => x.ParentFolderId == "" && x.OwnerId == userId &&
            !x.isDeleted).ToListAsync();
        }

        public async Task<string> GetPathPrefix(string folderId)
        {
            string path = string.Empty;
            var parent = await _fileManagementDbContext.FolderDetails.FirstOrDefaultAsync(x => x.Id == folderId && !x.isDeleted);
            while (parent != null)
            {
                path = parent.Name + "/" + path;
                parent = await _fileManagementDbContext.FolderDetails.FirstOrDefaultAsync(x => x.Id == parent.ParentFolderId && !x.isDeleted);
            }
            return path;
        }

        public async Task<bool> FolderNameExists(string folderPath, string ownerId)
        {
            return await _fileManagementDbContext.FolderDetails
                .AnyAsync(b => b.FolderPath == folderPath && b.OwnerId == ownerId && !b.isDeleted);
        }

        public async Task<FolderDetail> AddFolder(FolderDetail folderDetail)
        {
            await _fileManagementDbContext.FolderDetails.AddAsync(folderDetail);
            return folderDetail;
        }

        public FolderDetail UpdateFolder(FolderDetail folderDetail)
        {
            _fileManagementDbContext.FolderDetails.Update(folderDetail);
            return folderDetail;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _fileManagementDbContext.SaveChangesAsync() > 0);
        }
    }
}
