using IdentityFrameworkSportsstore.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IdentityFrameworkSportsstore.Controllers
{
    public class StoreController : Controller
    {
        #region Fields
        private IProductRepository _productRepository; 
        #endregion

        #region Constructor
        public StoreController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }
        #endregion

        #region Methods
        public IActionResult Index() {

            ICollection<Product> products = 
                _productRepository.GetByAvailability(Availability.OnlineOnly);

            products = _productRepository
                .GetByAvailability(Availability.ShopAndOnline).Concat(products).OrderBy(p => p.Name).ToList();

            return View(products);
        } 
        #endregion
    }
}