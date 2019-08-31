using BookStore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.Areas.Admin.Models.Categories
{
    public class CategoryInputModel
    {
        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [Display(Name = "Име")]
        public string Name { get; set; }
    }
}