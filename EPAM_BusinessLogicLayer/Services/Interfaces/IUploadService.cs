using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IUploadService
    {
        Task<string[]> UploadAsync(List<IFormFile> files);
    }
}