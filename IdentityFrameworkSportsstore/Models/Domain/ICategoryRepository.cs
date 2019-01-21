using System.Collections.Generic;

namespace IdentityFrameworkSportsstore.Models.Domain {
    public interface ICategoryRepository {
        #region Methods
        IEnumerable<Category> GetAll();
        Category GetById(int id); 
        #endregion
    }
}
