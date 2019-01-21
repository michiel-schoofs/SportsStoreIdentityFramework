using IdentityFrameworkSportsstore.Models.Domain;
using IdentityFrameworkSportsstore.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IdentityFrameworkSportsstore.Controllers {
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CartController : Controller
    {
        #region Fields
        private IProductRepository _productRepository;
        #endregion

        #region Constructor
        public CartController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }
        #endregion

        #region Methods
        public IActionResult Index(Cart cart) {

            if (cart.CartLines.Count() == 0)
                return View("EmptyCart");

            ViewData["Total"] = cart.CartLines.Sum(cl => cl.Total);
            return View(cart.CartLines);
        }

        public IActionResult CheckOut() {
            TempData["Error"] = "Not yet implemented";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Add(int id, int quantity, Cart cart) {
            Product pr = _productRepository.GetById(id);

            if (pr == null)
                TempData["Error"] = "We couldn't find the product you're trying to add";
            else {
                cart.AddLine(pr, quantity);
                TempData["Message"] = $"Product {pr.Name} has been added to the cart";
            }

            return RedirectToAction("Index", "Store");
        }

        [HttpPost]
        public IActionResult Remove(int id, Cart cart) {

            Product pr = _productRepository.GetById(id);
            if (pr != null) {
                TempData["Message"] = $"Deleted product {pr.Name} from cart.";
                cart.RemoveLine(pr);
            }

            return RedirectToAction(nameof(CartController.Index));
        }

        [HttpPost,ActionName("Plus")]
        public IActionResult Plus(int id,Cart cart) {
            CartLine cl = cart.CartLines.FirstOrDefault(cal => cal.Product.ProductId == id);
            cl.Quantity++;
            return RedirectToAction(nameof(CartController.Index));
        }

        [HttpPost, ActionName("Min")]
        public IActionResult Min(int id, Cart cart) {
            CartLine cl = cart.CartLines.FirstOrDefault(cal => cal.Product.ProductId == id);
            cl.Quantity--;
            return RedirectToAction(nameof(CartController.Index));
        }
        #endregion
    }
}