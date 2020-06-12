using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DataTransferObjects.Objects;

namespace Services.CategoryService.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<AuctionCategoryDto>> GetCategoriesAsync(int? limit, int? offset);
        Task<AuctionCategoryDto> GetCategoryAsync(Guid id);
        Task AddCategoriesAsync(IEnumerable<AuctionCategoryDto> categories);
        Task DeleteCategoryAsync(Guid id);
        Task DeleteCategoriesAsync(IEnumerable<Guid> categories);
    }
}