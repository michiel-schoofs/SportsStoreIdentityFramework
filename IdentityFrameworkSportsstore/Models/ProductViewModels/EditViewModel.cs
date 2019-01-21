using IdentityFrameworkSportsstore.Models.Domain;

namespace IdentityFrameworkSportsstore.Models.ProductViewModels {
    public class EditViewModel {
        #region Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
        public Availability Availability { get; set; }
        #endregion

        #region Constructors
        public EditViewModel(Product pr) {
            Name = pr.Name;
            Description = pr.Description;
            Price = pr.Price;
            InStock = pr.InStock;
            CategoryId = pr.Category.CategoryId;
            Availability = pr.Availability;
        }

        public EditViewModel() {
            InStock = true;
            Availability = Availability.ShopAndOnline;
        } 
        #endregion
    }
}
