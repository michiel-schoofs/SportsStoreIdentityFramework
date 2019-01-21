using IdentityFrameworkSportsstore.Models.Domain;
using IdentityFrameworkSportsstore.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;

namespace SportsStoreCrud.Controllers {
    public class ProductController : Controller {
        #region Field
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        #endregion

        #region Constructor
        public ProductController(IProductRepository repo, ICategoryRepository catRepo) {
            _productRepository = repo;
            _categoryRepository = catRepo;
        }
        #endregion

        #region Methods
        public IActionResult Index(int categoryId = 0) {
            ViewData["Categories"] = new SelectList(_categoryRepository.GetAllCategories(),
                nameof(Category.CategoryId)
                , nameof(Category.Name));

            IEnumerable<Product> products = _productRepository.GetAllProducts().OrderBy(p=>p.Name).ToList(); 

            if (categoryId != 0) {
                products = _productRepository.GetByCategory(categoryId).OrderBy(p => p.Name).ToList();
            }

            return View(products);
        }

        public IActionResult Edit(int id) {
            Product pr = _productRepository.GetProductById(id);

            if (pr == null)
                return NotFound();

            ViewData["Edit"] = true;
            AddSelectList();
            return View(new EditViewModel(pr));
        }

        [HttpPost]
        public IActionResult Edit(int id, EditViewModel m) {
            try {
                Product pr = _productRepository.GetProductById(id);
                pr.EditProduct(_categoryRepository.GetCategoryById(m.CategoryId), m.Name, m.Description,
                    m.Price, m.InStock, m.Availability);
                _productRepository.SaveChanges();
                TempData["Message"] = $"Product {pr.Name} bewerkt.";
            }catch (Exception e) {
                if(e is ArgumentException)
                    TempData["Error"] = $"Je gaf een ongeldige waarde:\n{e.Message}";
                else
                    TempData["Error"] = "Er ging iets fout met het editeren.";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create() {
            AddSelectList();
            ViewData["Edit"] = false;
            return View(nameof(Edit), new EditViewModel());
        }
        
        [HttpPost]
        public IActionResult Create(EditViewModel m) {
            try {
                Product pr = new Product(m.Name, m.Price, _categoryRepository.GetCategoryById(m.CategoryId),
                    m.Description, m.InStock, m.Availability);
                _productRepository.AddProduct(pr);
                _productRepository.SaveChanges();
            }
            catch (Exception e) {
                if (e is ArgumentException)
                    TempData["Error"] = $"Je gaf een ongeldige waarde:\n{e.Message}";
                else
                    TempData["Error"] = "Er ging iets fout met het maken van je produce.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id) {
            Product pr = _productRepository.GetProductById(id);

            if (pr == null)
                return NotFound();

            ViewData["Name"] = pr.Name;
            return View();
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id) {
            try {
                Product pr = _productRepository.GetProductById(id);

                if (pr == null) {
                    TempData["Message"] = "We couldn't find the product you were trying to delete";
                    throw new Exception();
                }

                _productRepository.RemoveProduct(pr);
                _productRepository.SaveChanges();

            }catch(Exception) {
                if (TempData["Message"] == null)
                    TempData["Message"] = "Something went wrong with deleting the product";
            }

            return RedirectToAction(nameof(Index));
        }

        private void AddSelectList() {
            ViewData["Categories"] = new SelectList(
                _categoryRepository.GetAllCategories(),
                nameof(Category.CategoryId),
                nameof(Category.Name),
                0
            );
        }
        #endregion
    }
}