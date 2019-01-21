using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using IdentityFrameworkSportsstore.Models.Domain;

namespace IdentityFrameworkSportsstore.Filters {
    public class CartSessionFilter : ActionFilterAttribute{

        private Cart _cart;
        private IProductRepository _productRepository;

        public CartSessionFilter(IProductRepository repository) {
            _productRepository = repository;
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            WriteCartToSession(_cart, context.HttpContext);
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            _cart = ReadCartFromSession(context.HttpContext);
            context.ActionArguments["cart"] = _cart;
            base.OnActionExecuting(context);
        }

        private Cart ReadCartFromSession(HttpContext context) {
            var s = context.Session.GetString("Cart");
            Cart c = new Cart();

            if (s != null) {
                foreach (CartLine cl in JsonConvert.DeserializeObject<Cart>(s).CartLines) {
                    c.AddLine(_productRepository.GetById(cl.Product.ProductId), cl.Quantity);
                }
            }

            return c;
        }

        private void WriteCartToSession(Cart cart,HttpContext context) {
            context.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }
    }
}
