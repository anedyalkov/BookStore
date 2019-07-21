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
        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        public string Name { get; set; }
    }
}
