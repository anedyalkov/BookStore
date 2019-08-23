using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Categories
{
    public class CategoryInputModel
    {
        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        [Display(Name = "Име")]
        public string Name { get; set; }
    }
}