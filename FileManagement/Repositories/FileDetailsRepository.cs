using FileManagement.AppDbContext;
using FileManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Repositories
{
    public class FileDetailsRepository : IFileDetailsRepository
    {
        private readonly FileManagementDbContext _fileManagementDbContext;

        public FileDetailsRepository(FileManagementDbContext fileManagementDbContext)
        {
            _fileManagementDbContext = fileManagementDbContext;
        }
        public async Task<FileDetail> GetFileDetails(string fileId, string userId)
        {
            return await _fileManagementDbContext.FileDetails.FirstOrDefaultAsync(x => x.Id == fileId && x.OwnerId == userId && !x.isDeleted);
        }
        public async Task<List<FileDetail>> GetChildFiles(string folderId, string userId)
        {
            return await _fileManagementDbContext.FileDetails.Where(x => x.FolderId == folderId && x.OwnerId == userId && !x.isDeleted).ToListAsync();
        }

        public async Task<FileDetail> GetFileDetails(string fileId)
        {
            return await _fileManagementDbContext.FileDetails.FirstOrDefaultAsync(x => x.Id == fileId && !x.isDeleted);
        }

        public async Task<FileDetail> GetFileDetailsByUserGroup(int userGroupId, string userId)
        {
            return await _fileManagementDbContext.FileDetails.FirstOrDefaultAsync(x => x.UserGroupId == userGroupId && x.OwnerId != userId
            && !x.isDeleted);
        }

        public async Task<List<FileDetail>> GetChildFiles(string folderId)
        {
            return await _fileManagementDbContext.FileDetails.Where(x => x.FolderId == folderId && !x.isDeleted).ToListAsync();
        }
        public async Task<List<FileDetail>> GetrootFiles(string userId)
        {
            return await _fileManagementDbContext.FileDetails.Where(x => x.FolderId == "" && x.OwnerId == userId && !x.isDeleted).ToListAsync();
        }

        public async Task<bool> FileNameExists(string FilePath, string ownerId)
        {
            return await _fileManagementDbContext.FileDetails
                .AnyAsync(b => b.FilePath == FilePath && b.OwnerId == ownerId && !b.isDeleted);
        }

        public async Task<FileDetail> AddFile(FileDetail fileDetail)
        {
            await _fileManagementDbContext.FileDetails.AddAsync(fileDetail);
            return fileDetail;
        }

        public FileDetail UpdateFileDetails(FileDetail fileDetail)
        {
            _fileManagementDbContext.FileDetails.Update(fileDetail);
            return fileDetail;
        }

        public async Task<FileType> AddFileType(FileType fileType)
        {
            await _fileManagementDbContext.FileTypes.AddAsync(fileType);
            return fileType;
        }

        public  FileType UpdateFileType(FileType fileType)
        {
            _fileManagementDbContext.FileTypes.Update(fileType);
            return fileType;
        }

        public async Task<FileType> GetFileTypeByIdAsync(int fileTypeId)
        {
            return await _fileManagementDbContext.FileTypes.FirstOrDefaultAsync(x => x.Id == fileTypeId && !x.isDeleted);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _fileManagementDbContext.SaveChangesAsync() > 0);
        }
    }
}
