using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Moq;
using NUnit.Framework;
using Services.CategoryService.Interfaces;
using Services.CategoryService.Service;
using Services.DataTransferObjects.Objects;
using Services.DataTransferObjects.Profiles;

namespace EPAM_API.Tests
{
    public class CategoryServiceTests
    {
        private readonly List<Category> _categories = new List<Category>
        {
            new Category { Id = Guid.Empty, Name = "category1", Description = "desc1", Auctions = null},
            new Category { Id = Guid.Empty, Name = "category2", Description = "desc2", Auctions = null }
        };

        private ICategoryService _categoryService;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void Setup()
        {
            // Assign
            _mockUnitOfWork = Utility.MockUnitOfWork();
            var mapper = new MapperConfiguration(opts => { opts.AddProfile( new CategoryProfile()); }).CreateMapper();

            _categoryService = new CategoryService(mapper, _mockUnitOfWork.Object);
        }

        [Test]
        public async Task AddCategory_CorrectDto_ReturnsCategory()
        {
            // Assign
            _mockUnitOfWork.Setup(m => m.InsertAsync(It.IsAny<Category>(), CancellationToken.None))
                .ReturnsAsync(_categories[0]);

            // Act
            var result = await _categoryService.AddCategoryAsync(new AuctionCategoryDto()
            {
                Name = _categories[0].Name,
                Description = _categories[0].Description
            });

            // Assert
            Assert.True(
                result.Name == _categories[0].Name
                && result.Description == _categories[0].Description
            );
        }
    }
}