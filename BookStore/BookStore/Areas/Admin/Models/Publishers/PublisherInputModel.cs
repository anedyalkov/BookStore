using BookStore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Publishers
{
    public class PublisherInputModel
    {
        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.PublisherNameMaxLength)]
        [Display(Name = "Име")]
        public string Name { get; set; }
    }
}
