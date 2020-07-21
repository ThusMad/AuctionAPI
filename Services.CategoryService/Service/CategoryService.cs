using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.CategoryService.Interfaces;
using Services.DataTransferObjects.Objects;
using Services.Infrastructure.Exceptions;

namespace Services.CategoryService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AuctionCategoryDto>> GetCategoriesAsync(int? limit, int? offset)
        {
            var categories = await _unitOfWork.GetAll<Category>().ToListAsync();
            return _mapper.Map<IEnumerable<Category>, IEnumerable<AuctionCategoryDto>>(categories);
        }

        public async Task<AuctionCategoryDto> GetCategoryAsync(Guid id)
        {
            var categories = await _unitOfWork.GetByIdAsync<Category>(id);
            return _mapper.Map<Category, AuctionCategoryDto>(categories);
        }

        public async Task AddCategoriesAsync(IEnumerable<AuctionCategoryDto> categories)
        {
            var newCategories = new List<AuctionCategoryDto>();

            foreach (var auctionCategoryDto in categories)
            {
                if (!await _unitOfWork.AnyAsync<Category>(c => c.Name == auctionCategoryDto.Name))
                {
                    newCategories.Add(auctionCategoryDto);
                }
            }

            _mapper.Map<IEnumerable<AuctionCategoryDto>, IEnumerable<Category>>(newCategories)
                .ToList()
                .ForEach(async a => await _unitOfWork.InsertAsync(a));

            await _unitOfWork.CommitAsync();
        }

        public async Task<AuctionCategoryDto> AddCategoryAsync(AuctionCategoryDto category)
        {
            if (await _unitOfWork.AnyAsync<Category>(c => c.Name == category.Name))
            {
                new UserException(400, "Category with following name already exists!");
            }

            var categoryEntity = _mapper.Map<AuctionCategoryDto, Category>(category);

            var result = await _unitOfWork.InsertAsync(categoryEntity, CancellationToken.None);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<Category, AuctionCategoryDto>(result);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = _unitOfWork.Find<Category>(c => c.Id == id);
            if (category == null)
            {
                throw new ItemNotFountException(nameof(id), "Category with following id not found");
            }

            _unitOfWork.Remove(category);

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCategoriesAsync(IEnumerable<Guid> categories)
        {
            var categoriesToDelete = new List<Category>();

            foreach (var id in categories)
            {
                var category = _unitOfWork.Find<Category>(c => c.Id == id);
                if (category != null)
                {
                    categoriesToDelete.AddRange(category);
                }
            }

            _unitOfWork.RemoveRange(categoriesToDelete);

            await _unitOfWork.CommitAsync();
        }
    }
}
