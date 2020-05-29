using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;

namespace EPAM_BusinessLogicLayer.Services
{
    class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AuctionCategoryDto> GetCategories(int? limit, int? offset)
        {
            var categories = _unitOfWork.GetAll<Category>().AsEnumerable();
            return _mapper.Map<IEnumerable<Category>, IEnumerable<AuctionCategoryDto>>(categories);
        }

        public AuctionCategoryDto GetCategory(Guid id)
        {
            var categories = _unitOfWork.GetById<Category>(id);
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

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = _unitOfWork.Find<Category>(c => c.Id == id);
            if (category == null)
            {
                throw new ItemNotFountException(nameof(id), "Category with following id not found");
            }

            _unitOfWork.Delete(category);

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

            _unitOfWork.DeleteRange(categoriesToDelete);

            await _unitOfWork.CommitAsync();
        }
    }
}
