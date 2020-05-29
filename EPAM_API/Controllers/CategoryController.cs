using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IActionResult GetCategory(Guid id)
        {
            var category = _categoryService.GetCategory(id);

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
        public IActionResult GetCategories(int? limit, int? offset)
        {
            var categories = _categoryService.GetCategories(limit, offset);

            return Ok(JsonSerializer.Serialize(categories));
        }
    }
}