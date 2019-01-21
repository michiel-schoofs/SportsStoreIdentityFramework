using System;
using Newtonsoft.Json;

namespace IdentityFrameworkSportsstore.Models.Domain {
    [JsonObject(MemberSerialization.OptIn)]
    public class CartLine {
        #region Fields
        [JsonProperty]
        private int _quantity;
        [JsonProperty]
        private Product _product;
        #endregion

        #region Properties
        public Product Product {
            get => _product;
            set {
                _product = value ?? throw new ArgumentException("You must specify a product");
            }
        }

        public int Quantity {
            get => _quantity;
            set {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be positive");
                _quantity = value;
            }
        }

        public decimal Total => Product.Price * Quantity;

        #endregion

        #region Constructors
        public CartLine(Product product, int quantity) {
            Product = product;
            Quantity = quantity;
        }

        [JsonConstructor]
        // Added for EF because EF cannot set navigation properties through constructor parameters
        protected CartLine() {

        }
        #endregion
    }
}