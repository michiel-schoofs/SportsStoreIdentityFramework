using System.ComponentModel.DataAnnotations;

namespace IdentityFrameworkSportsstore.Models.Domain {
    public enum Availability {
        #region Enum
        [Display(Name ="Shop only")]
        ShopOnly,
        [Display(Name ="Online only")]
        OnlineOnly,
        [Display(Name ="Shop and online")]
        ShopAndOnline 
        #endregion
    }
}
