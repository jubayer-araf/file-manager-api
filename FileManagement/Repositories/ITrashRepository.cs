using FileManagement.Entities;

namespace FileManagement.Repositories
{
    public interface ITrashRepository
    {
        Task<FileDetail> GetFileDetails(string fileId, string userId);
        Task<FolderDetail> GetFolderDetails(string folderId, string userId);
        Task<List<FolderDetail>> GetChildFolders(string folderId, string userId);
        Task<List<FileDetail>> GetChildFiles(string folderId, string userId);
        Task<List<FolderDetail>> GetRootFolders(string userId);
        Task<List<FileDetail>> GetDeletedChildFileWithoutFolder(string userId);
        Task<List<FolderDetail>> GetDeletedFolders(string userId);
        Task<bool> SaveChangesAsync();
    }
}
