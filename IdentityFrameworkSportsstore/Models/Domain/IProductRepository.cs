using System.Collections.Generic;
namespace IdentityFrameworkSportsstore.Models.Domain {
    public interface IProductRepository {
        #region Methods
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByCategory(int catId);
        ICollection<Product> GetByAvailability(Availability availability);
        Product GetById(int id);
        void Add(Product pr);
        void Delete(Product pr);
        void SaveChanges(); 
        #endregion
    }
}
