using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Authors
{
    public class AuthorInputModel
    {
        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
    }
}
