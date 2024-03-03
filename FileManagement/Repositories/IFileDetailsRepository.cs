using FileManagement.Entities;

namespace FileManagement.Repositories
{
    public interface IFileDetailsRepository
    {
        Task<FileDetail> GetFileDetails(string FfileId, string userId);
        Task<List<FileDetail>> GetChildFiles(string fileId, string userId);
        Task<FileDetail> GetFileDetails(string fileId);
        Task<FileDetail> GetFileDetailsByUserGroup(int userGroupId, string userId);
        Task<List<FileDetail>> GetChildFiles(string fileId);
        Task<List<FileDetail>> GetrootFiles(string userId);
        Task<bool> FileNameExists(string filePath, string ownerId);
        Task<FileDetail> AddFile(FileDetail fileDetail);
        FileDetail UpdateFileDetails(FileDetail fileDetail);
        Task<FileType> AddFileType(FileType fileType);
        FileType UpdateFileType(FileType fileType);
        Task<FileType> GetFileTypeByIdAsync(int fileTypeId);
        Task<bool> SaveChangesAsync();
    }
}
