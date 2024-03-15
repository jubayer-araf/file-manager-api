using FileManagement.Entities;
using FileManagement.Models;
using FileManagement.Repositories;
using FileManagement.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsFileController : ControllerBase
    {
        private readonly ICustomAuthorizeService _customAuthorizeService;
        private readonly ISettingsFileService _settingsFileService;
        private readonly ISettingsFileRepository _settingsFileRepository;

        public SettingsFileController(ICustomAuthorizeService customAuthorizeService,
            ISettingsFileService settingsFileService,
            ISettingsFileRepository settingsFileRepository)
        {
            _customAuthorizeService = customAuthorizeService;
            _settingsFileService = settingsFileService;
            _settingsFileRepository = settingsFileRepository;
        }

        // GET api/<SettingsFileController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null)
                {
                    var currentFileDetails = await _settingsFileRepository.GetSettingsFile(id, applicationUser.Id);
                    if (currentFileDetails != null)
                    {
                        var fileBytes = await _settingsFileService.GetFileFromStorage(currentFileDetails.Id);
                        if (fileBytes != null)
                        {
                            Stream stream = new MemoryStream(fileBytes);
                            return File(stream, "application/octet-stream", $"{currentFileDetails.Name}{currentFileDetails.Extension}");
                        }
                        return NotFound();
                    }
                    return NotFound();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<SettingsFileController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            try
            {
                ApplicationUser applicationUser = await _customAuthorizeService.GetUserAsync(ControllerContext);
                if (applicationUser != null && file != null)
                {
                    SettingsFile settingsFile = new SettingsFile
                    {
                        Name = file.Name,
                        Size = (int)file.Length,
                        FileTypeId = 10,
                        Extension = Path.GetExtension(file.Name),
                        OwnerId = applicationUser.Id,
                        CreatedByUserId = applicationUser.Id,
                        LastUpdatedByUserId = applicationUser.Id,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        isDeleted = false
                    };

                    var savedSettingsFile = await _settingsFileRepository.AddSettingsFile(settingsFile);

                    await _settingsFileService.StoreFileAsync(file, savedSettingsFile.Id);

                    return Ok();

                }

                return Unauthorized();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
