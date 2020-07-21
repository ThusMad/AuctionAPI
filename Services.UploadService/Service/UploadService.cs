using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Helpers.Utilities;
using Services.Infrastructure.Exceptions;
using Services.UploadService.Interfaces;

namespace Services.UploadService.Service
{
    /// <summary>
    /// ///<inheritdoc cref="IUploadService"/>
    /// </summary>
    public class UploadService : IUploadService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        private const string RootFolder = "wwwroot";
        /// <summary>
        /// 
        /// </summary>
        private const string ImagesFolder = "images";
        /// <summary>
        /// 
        /// </summary>
        private static string ImagesPath => Path.Combine(RootFolder, ImagesFolder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        public UploadService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(UploadService));
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
                paths.Add(await SaveFile(file, pathToSave));
            }

            return paths.ToArray();
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            return await SaveFile(file, ImagesPath);
        }

        public async Task RemoveAsync(List<string> files)
        {
            foreach (var file in files)
            {
                try
                {
                    await RemoveFileAsync(file, ImagesPath);
                }
                catch (FileNotFoundException e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        public async Task RemoveAsync(string file)
        {
            await RemoveFileAsync(file, ImagesPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        private static Task RemoveFileAsync(string filepath, string rootPath)
        {
            var fileName = Path.GetFileName(filepath);
            var path = Path.Combine(rootPath, fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {fileName} not present");
            }

            return Task.Run(() => { File.Delete(path); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static async Task<string> SaveFile(IFormFile file, string path)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var extension = Path.GetExtension(fileName);
            var hashedName = Utility.GetHashString(fileName + DateTimeOffset.UtcNow.ToUnixTimeSeconds()) + extension;
            var fullPath = Path.Combine(path, hashedName);

            var dbPath = "https://localhost:44391/uploads/" + hashedName;

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return dbPath;
        }
    }
}
