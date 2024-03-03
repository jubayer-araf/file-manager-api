using FileManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileManagement.Services
{
    public interface ICustomAuthorizeService
    {
        Task<ApplicationUser> GetUserAsync(ControllerContext context);
    }
}
