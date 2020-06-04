using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DataTransferObjects.Objects;

namespace Services.CategoryService.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<AuctionCategoryDto> GetCategories(int? limit, int? offset);
        AuctionCategoryDto GetCategory(Guid id);
        Task AddCategoriesAsync(IEnumerable<AuctionCategoryDto> categories);
        Task DeleteCategoryAsync(Guid id);
        Task DeleteCategoriesAsync(IEnumerable<Guid> categories);
    }
}