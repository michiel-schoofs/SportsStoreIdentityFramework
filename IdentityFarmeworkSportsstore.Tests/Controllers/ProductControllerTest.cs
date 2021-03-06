﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using IdentityFrameworkSportsstore.Models.Domain;
using IdentityFrameworkSportsstore.Controllers;
using IdentityFrameworkSportsstore.Tests.Data;
using IdentityFrameworkSportsstore.Models.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IdentityFrameworkSportsstore.Tests.Controllers {
    public class ProductControllerTest {
        private readonly DummyApplicationDbContext _dummyContext;
        private readonly ProductController _productController;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Product _runningShoes;
        private readonly int _runningShoesId;

        public ProductControllerTest() {
            _dummyContext = new DummyApplicationDbContext();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _runningShoes = _dummyContext.RunningShoes;
            _runningShoesId = _runningShoes.ProductId;
            _productController = new ProductController(_mockProductRepository.Object, _mockCategoryRepository.Object) {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }

        #region == Index ==
        [Fact]
        public void Index_AllCategories_PassesAllProductsSortedByNameInModel() {
            _mockProductRepository.Setup(p => p.GetAll()).Returns(_dummyContext.Products);
            var result = _productController.Index() as ViewResult;
            List<Product> products = (result?.Model as IEnumerable<Product>)?.ToList();
            Assert.Equal(11, products.Count);
            Assert.Equal("Bling-bling King", products[0].Name);
            Assert.Equal("Unsteady chair", products[10].Name);
        }

        [Fact]
        public void Index_ExistingCategory_PassesSelectListWithAllCategoriesSortedByNameInViewData() {
            _mockCategoryRepository.Setup(p => p.GetAll()).Returns(_dummyContext.Categories);
            var result = _productController.Index() as ViewResult;
            var categories = result?.ViewData["Categories"] as SelectList;
            Assert.Equal(3, categories.Count());
        }

        [Fact]
        public void Index_ExistingCategory_PassesAllProductsFromThatCategorySortedByNameInModel() {
            _mockProductRepository.Setup(p => p.GetByCategory(1)).Returns(_dummyContext.Soccer.Products);
            var result = _productController.Index(1) as ViewResult;
            List<Product> products = (result?.Model as IEnumerable<Product>)?.ToList();
            Assert.Equal(4, products.Count);
            Assert.Equal("Corner flags", products[0].Name);
        }
        #endregion

        #region == Edit ==
        [Fact]
        public void EditHttpGet_ValidProductId_PassesProductDetailsInAnEditViewModelToView() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            _mockCategoryRepository.Setup(c => c.GetAll()).Returns(_dummyContext.Categories);
            var result = _productController.Edit(1) as ViewResult;
            var productVm = result?.Model as EditViewModel;
            Assert.Equal("Running shoes", productVm?.Name);
        }

        [Fact]
        public void EditHttpget_ValidProductId_PassesSelectListWithAllCategoriesSortedByNameInViewData() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            _mockCategoryRepository.Setup(c => c.GetAll()).Returns(_dummyContext.Categories);
            var result = _productController.Edit(1) as ViewResult;
            var categories = result?.ViewData["Categories"] as SelectList;
            Assert.Equal(3, categories.Count());
        }

        [Fact]
        public void EditHttpGet_ProductNotFound_ReturnsNotFound() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(null as Product);
            var result = _productController.Edit(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EditHttpPost_ValidEdit_UpdatesAndPersistsTheProduct() {
            _mockProductRepository.Setup(p => p.GetById(2)).Returns(_dummyContext.RunningShoes);
            _mockCategoryRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(_dummyContext.Soccer);
            var productVm = new EditViewModel(_dummyContext.RunningShoes) {
                Name = "RunningShoesGewijzigd",
                Price = 1000
            };
            _productController.Edit(2, productVm);
            Assert.Equal("RunningShoesGewijzigd", _runningShoes.Name);
            Assert.Equal(1000, _runningShoes.Price);
            Assert.Equal("Protective and fashionable", _runningShoes.Description);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EditHttpPost_ValidEdit_RedirectsToIndex() {
            _mockProductRepository.Setup(p => p.GetById(2)).Returns(_dummyContext.RunningShoes);
            _mockCategoryRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(_dummyContext.Soccer);
            var productVm = new EditViewModel(_runningShoes);
            var result = _productController.Edit(2, productVm) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void EditHttpPost_InValidEdit_DoesNotChangeNorPersistProduct() {
            _mockProductRepository.Setup(m => m.GetById(2)).Returns(_dummyContext.RunningShoes);
            var productVm = new EditViewModel(_dummyContext.RunningShoes) { Price = -1 };
            _productController.Edit(2, productVm);
            var runningShoes = _dummyContext.RunningShoes;
            Assert.Equal("Running shoes", runningShoes.Name);
            Assert.Equal(95, runningShoes.Price);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void EditHttpPost_InValidEdit_RedirectsToActionIndex() {
            _mockProductRepository.Setup(m => m.GetById(2)).Returns(_dummyContext.RunningShoes);
            var productVm = new EditViewModel(_dummyContext.RunningShoes) { Price = -1 };
            var action = _productController.Edit(2, productVm) as RedirectToActionResult;
            Assert.Equal("Index", action?.ActionName);
        }

        [Fact]
        public void EditHttpPost_ProductNotFound_ReturnsNotFoundResult() {
            _mockProductRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(null as Product);
            var nfr = _productController.Edit(2, new EditViewModel(_dummyContext.RunningShoes));
            Assert.IsType<NotFoundResult>(nfr);
        }

        [Fact]
        public void EditHttpPost_ModelStateErrors_DoesNotChangeNorPersistTheProduct() {
            Product pro = _dummyContext.Football;
            _mockProductRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(pro);
            EditViewModel evm = new EditViewModel(_dummyContext.Football) { Price = 50 };
            _productController.ModelState.AddModelError("", "sqdklfmjqsd");

            _productController.Edit(2, evm);

            Assert.NotEqual(50, pro.Price);
            _mockProductRepository.Verify(p => p.SaveChanges(), Times.Never);
        }

        [Fact]
        public void EditHttpPost_ModelStateErrors_PassesEditViewModelInViewResultModel() {
            _mockProductRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(_dummyContext.Football);
            EditViewModel evm = new EditViewModel(_dummyContext.Football) { Price = 50 };
            _productController.ModelState.AddModelError("", "sqdklfmjqsd");

            var x = _productController.Edit(2, evm);

            Assert.IsType<ViewResult>(x);
            Assert.Equal(evm, (x as ViewResult).Model);
        }

        [Fact]
        public void EditHttpPost_ModelStateErrors_PassesSelectListsInViewData() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            _mockCategoryRepository.Setup(c => c.GetAll()).Returns(_dummyContext.Categories);
            EditViewModel evm = new EditViewModel(_dummyContext.Football) { Price = 50 };
            _productController.ModelState.AddModelError("", "sqdklfmjqsd");

            var result = _productController.Edit(1, evm);
            Assert.IsType<ViewResult>(result);

            var categories = (result as ViewResult)?.ViewData["Categories"] as SelectList;

            Assert.Equal(3, categories.Count());
        }
        #endregion

        #region == Create ==
        [Fact]
        public void CreateHttpGet_PassesDetailsOfANewProductInAnEditViewModelToView() {
            _mockCategoryRepository.Setup(c => c.GetAll()).Returns(_dummyContext.Categories);
            var result = _productController.Create() as ViewResult;
            var productVm = result?.Model as EditViewModel;
            Assert.Null(productVm?.Name);
        }

        [Fact]
        public void CreateHttpPost_ValidProduct_RedirectsToIndex() {
            _mockCategoryRepository.Setup(c => c.GetById(1)).Returns(_dummyContext.Soccer);
            var productVm = new EditViewModel() {
                CategoryId = 1,
                Name = "nieuw product",
                Price = 10
            };
            var result = _productController.Create(productVm) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void CreateHttpPost_ValidProduct_AddsNewProductToRepository() {
            _mockProductRepository.Setup(p => p.Add(It.IsNotNull<Product>()));
            _mockCategoryRepository.Setup(p => p.GetById(It.IsAny<int>())).Returns(_dummyContext.Soccer);
            var productVm = new EditViewModel() {
                CategoryId = 1,
                Name = "nieuw product",
                Price = 10
            };
            _productController.Create(productVm);
            _mockProductRepository.Verify(m => m.Add(It.IsNotNull<Product>()), Times.Once);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateHttpPost_InvalidProduct_DoesNotCreateNorPersistsProduct() {
            _mockProductRepository.Setup(m => m.Add(It.IsAny<Product>()));
            var productVm = new EditViewModel() {
                CategoryId = 1,
                Name = "nieuw product",
                Price = -10
            };
            _productController.Create(productVm);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Never());
            _mockProductRepository.Verify(m => m.Add(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void CreateHttpPost_InvalidProduct_RedirectsToActionIndex() {
            var productVm = new EditViewModel() {
                CategoryId = 1,
                Name = "nieuw product",
                Price = -10
            };
            var action = _productController.Create(productVm) as RedirectToActionResult;
            Assert.Equal("Index", action?.ActionName);
        }
        #endregion

        #region == Delete ==
        [Fact]
        public void DeleteHttpGet_ProductFound_PassesProductNameInViewDataToView() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            var result = _productController.Delete(1) as ViewResult;
            Assert.Equal("Running shoes", result?.ViewData["Name"]);
        }

        [Fact]
        public void DeleteHttpGet_ProductNotFound_ReturnsNotFound() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(null as Product);
            var result = _productController.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteHttpPost_ProductFound_DeletesProduct() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            _mockProductRepository.Setup(p => p.Delete(_dummyContext.RunningShoes));
            _productController.DeleteConfirmed(1);
            _mockProductRepository.Verify(m => m.Delete(_dummyContext.RunningShoes), Times.Once);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteHttpPost_SuccessfullDelete_RedirectsToIndex() {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            var result = _productController.DeleteConfirmed(_runningShoesId) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void DeleteHttpPost_UnsuccessfullDelete_RedirectsToIndex() {
            _mockProductRepository.Setup(p => p.GetById(1)).Throws<ArgumentException>();
            var result = _productController.DeleteConfirmed(1) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}