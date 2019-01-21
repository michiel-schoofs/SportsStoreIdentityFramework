using Microsoft.EntityFrameworkCore;
using IdentityFrameworkSportsstore.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace IdentityFrameworkSportsstore.Data.Repositories {
    public class ProductRepository : IProductRepository {
        #region Fields
        private readonly DbSet<Product> _products;
        private ApplicationDbContext _context; 
        #endregion

        #region Constructor
        public ProductRepository(ApplicationDbContext context) {
            _context = context;
            _products = _context.Products;
        } 
        #endregion

        #region Methods
        public void Add(Product pr) {
            _context.Products.Add(pr);
        }

        public IEnumerable<Product> GetAll() {
            return _context.Products.ToList();
        }

        public ICollection<Product> GetByAvailability(Availability availability) {
            return _context.Products.Where(p => p.Availability.Equals(availability)).ToList();
        }

        public IEnumerable<Product> GetByCategory(int catId) {
            return _context.Products.Include(p => p.Category).Where(p => p.Category.CategoryId == catId).ToList();
        }

        public Product GetById(int id) {
            return _context.Products.Include(p=>p.Category).FirstOrDefault(p => p.ProductId == id);
        }

        public void Delete(Product pr) {
            _context.Products.Remove(pr);
        }

        public void SaveChanges() {
            _context.SaveChanges();
        } 
        #endregion
    }
}
