using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
// Make SportsStore.tests a friendly assembly so it can access the internal properties of this class
[assembly: InternalsVisibleTo("SportsStore.Tests")]

namespace IdentityFrameworkSportsstore.Models.Domain {
    [JsonObject(MemberSerialization.OptIn)]
    public class Product {
        #region Fields
        private string _name;
        private decimal _price;
        private Category _category;
        #endregion

        #region Properties
        [JsonProperty]
        public int ProductId { get;  set; }

        public string Name {
            get => _name;
            private set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Category must have a name, and the name should not exceed 100 characters");
                _name = value;
            }
        }

        public decimal Price {
            get => _price;
            private set {
                if (value < 1 || value > 3000)
                    throw new ArgumentException("Price must be in the range 1 - 3000");
                _price = value;
            }
        }

        public Category Category {
            get => _category;
            private set {
                _category = value ?? throw new ArgumentException("You must specify a category for a product");
            }
        }

        public string Description { get; private set; }
        public bool InStock { get; private set; }
        public Availability Availability { get; private set; }
        #endregion

        #region Constructors
        public Product(string name, decimal price, Category category, string description = null, bool inStock = true,Availability availability=Availability.ShopAndOnline) {
            InStock = inStock;
            Name = name;
            Price = price;
            Availability = availability;
            Description = description;
            Category = category;
            category.AddProduct(this);
        }

        [JsonConstructor]
        public Product(int productId) {
            ProductId = productId;
        }

        // Added for EF because EF cannot set navigation properties through constructor parameters
        private Product() {
        }
        #endregion

        #region Methods
        public override bool Equals(object obj) {
            return obj is Product p && p.ProductId == ProductId;
        }

        public override int GetHashCode() {
            return HashCode.Combine(ProductId);
        }

        public void EditProduct(Category cat, string name,string desc,decimal price, bool inStock,Availability availability) {
            Category = cat;
            Name = name;
            Description = desc;
            Price = price;
            InStock = inStock;
            Availability = availability;
        }
        #endregion
    }
}