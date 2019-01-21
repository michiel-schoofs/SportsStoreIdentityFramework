using Microsoft.EntityFrameworkCore;
using IdentityFrameworkSportsstore.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace IdentityFrameworkSportsstore.Data.Repositories {
    public class CategoryRepository : ICategoryRepository {
        #region Field
        private DbSet<Category> _categories; 
        #endregion

        #region Constructor
        public CategoryRepository(ApplicationDbContext context) {
            _categories = context.Categories;
        } 
        #endregion

        #region Methods
        public IEnumerable<Category> GetAll() {
            return _categories.ToList();
        }

        public Category GetById(int id) {
            return _categories.FirstOrDefault(c => c.CategoryId == id);
        } 
        #endregion
    }
}
