using System.Collections.Generic;

namespace IdentityFrameworkSportsstore.Models.Domain {
    public interface ICategoryRepository {
        #region Methods
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id); 
        #endregion
    }
}
