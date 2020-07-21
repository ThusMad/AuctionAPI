using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CategoryService.Interfaces;
using Services.DataTransferObjects.Objects;

namespace EPAM_API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryAsync(id);

            return Ok(JsonSerializer.Serialize(category));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCategories([FromBody] IEnumerable<AuctionCategoryDto> categories)
        {
            await _categoryService.AddCategoriesAsync(categories);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return Ok();
        }

        [Authorize]
        [HttpDelete, Route("deleteRange")]
        public async Task<IActionResult> DeleteCategories([FromBody] IEnumerable<Guid> categories)
        {
            await _categoryService.DeleteCategoriesAsync(categories);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetCategories(int? limit, int? offset)
        {
            var categories = await _categoryService.GetCategoriesAsync(limit, offset);

            return Ok(JsonSerializer.Serialize(categories));
        }
    }
}