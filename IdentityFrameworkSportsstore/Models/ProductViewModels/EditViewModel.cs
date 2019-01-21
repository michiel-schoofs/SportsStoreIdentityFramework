using IdentityFrameworkSportsstore.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityFrameworkSportsstore.Models.ProductViewModels {
    public class EditViewModel {
        #region Properties
        [Required(ErrorMessage ="You have to provide a name for the product")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="The name has to be 5-100 characters long")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage ="You have to provide a price for the product")]
        [Range(1.0,3000.0,ErrorMessage ="Price has to be between 1 and  3000")]
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        [Required(ErrorMessage ="The product must have a category")]
        public int CategoryId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? AvailableTill { get; set; }
        [Required(ErrorMessage ="The product must have an availability set")]
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
            AvailableTill = pr.AvailableTill;
        }

        public EditViewModel() {
            InStock = true;
            Availability = Availability.ShopAndOnline;
        } 
        #endregion
    }
}
