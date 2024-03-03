using FileManagement.Entities;

namespace FileManagement.Repositories
{
    public interface IFolderRepository
    {
        Task<FolderDetail> GetFolderDetails(string folderId, string userId);
        Task<List<FolderDetail>> GetChildFolders(string folderId, string userId);
        Task<FolderDetail> GetFolderDetails(string folderId);
        Task<List<FolderDetail>> GetChildFolders(string folderId);
        Task<List<FolderDetail>> GetRootFolders(string userId);
        Task<string> GetPathPrefix(string folderId);
        Task<bool> FolderNameExists(string folderPath, string ownerId);
        Task<FolderDetail> AddFolder(FolderDetail folderDetail);
        FolderDetail UpdateFolder(FolderDetail folderDetail);
        Task<bool> SaveChangesAsync();
    }
}
