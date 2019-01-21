using System.Collections.Generic;
namespace IdentityFrameworkSportsstore.Models.Domain {
    public interface IProductRepository {
        #region Methods
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetByCategory(int catId);
        ICollection<Product> GetByAvailability(Availability availability);
        Product GetProductById(int id);
        void AddProduct(Product pr);
        void RemoveProduct(Product pr);
        void SaveChanges(); 
        #endregion
    }
}
