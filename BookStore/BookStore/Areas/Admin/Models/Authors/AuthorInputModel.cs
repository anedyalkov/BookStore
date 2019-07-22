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
        [Required]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string LastName { get; set; }
    }
}
