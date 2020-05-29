using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EPAM_BusinessLogicLayer.Services
{
    class UploadService : IUploadService
    {
        private const string RootFolder = "wwwroot";
        private const string ImagesFolder = "images";

        public UploadService()
        {

        }

        public async Task<string[]> UploadAsync(List<IFormFile> files)
        {
            if (files.Any(f => f.Length == 0))
            { 
                throw new UserException(200, "No images to upload");
            }

            var folderName = Path.Combine(RootFolder, ImagesFolder);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var paths = new List<string>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var extension = Path.GetExtension(fileName);
                var hashedName = Utility.GetHashString(fileName + Utility.DateTimeToUnixTimestamp(DateTime.UtcNow)) + extension;
                var fullPath = Path.Combine(pathToSave, hashedName);

                var dbPath = "https://localhost:44391/uploads/" + hashedName; 

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                paths.Add(dbPath);
            }

            return paths.ToArray();
        }
    }
}
