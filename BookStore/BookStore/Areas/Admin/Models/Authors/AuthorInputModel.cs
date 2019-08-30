using BookStore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.Areas.Admin.Models.Authors
{
    public class AuthorInputModel
    {
        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
    }
}
