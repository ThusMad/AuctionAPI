using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.UploadService.Interfaces
{
    /// <summary>
    /// Service that provides methods that allow working with files
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Uploads and saves provided files
        /// </summary>
        /// <param name="files">Files to be saved</param>
        /// <returns>Collection of strings that contains urls</returns>
        Task<string[]> UploadAsync(List<IFormFile> files);
        /// <summary>
        /// Uploads and saves provided file
        /// </summary>
        /// <param name="file">File to be saved</param>
        /// <returns>Url string</returns>
        Task<string> UploadAsync(IFormFile file);
        /// <summary>
        /// Removes files by their links
        /// </summary>
        /// <param name="files">Files to delete</param>
        /// <returns></returns>
        Task RemoveAsync(List<string> files);
        /// <summary>
        /// Remove file by their url
        /// </summary>
        /// <param name="file">File to delete</param>
        /// <returns></returns>
        Task RemoveAsync(string file);
    }
}