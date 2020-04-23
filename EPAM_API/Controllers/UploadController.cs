using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPAM_API.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var folderName = Path.Combine("wwwroot", "images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            List<string> paths = new List<string>();

            if (files.Any(f => f.Length == 0))
            {
                return BadRequest();
            }

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var extension = Path.GetExtension(fileName);
                var hashedName = EPAM_BusinessLogicLayer.Utility.GetHashString(fileName + Request.Query["timestamp"]).ToLower() + extension;
                var fullPath = Path.Combine(pathToSave, hashedName);
                var dbPath = "uploads/" + hashedName; //you can add this path to a list and then return all dbPaths to the client if require

                await using var stream = new FileStream(fullPath, FileMode.Create);
                    await file.CopyToAsync(stream);

                paths.Add(dbPath);
            }

            return Ok(JsonSerializer.Serialize(paths));
        }
    }
}