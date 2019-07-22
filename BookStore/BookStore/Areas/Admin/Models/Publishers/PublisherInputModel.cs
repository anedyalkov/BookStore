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
        [Required]
        [MaxLength(DataConstants.PublisherNameMaxLength)]
        public string Name { get; set; }
    }
}
