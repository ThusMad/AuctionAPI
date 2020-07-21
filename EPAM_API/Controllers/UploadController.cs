using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UploadService.Interfaces;

namespace EPAM_API.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var paths = await _uploadService.UploadAsync(files);

            return Ok(JsonSerializer.Serialize(paths));
        }
    }
}