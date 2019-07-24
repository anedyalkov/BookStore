using BookStore.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Books
{
    public class BookInputModel
    {

        [Required]
        [Display(Name = "Заглавие")]
        [MaxLength(DataConstants.BookTitleMaxLength)]
        public string Title { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public int PublisherId { get; set; }

        [Required]
        [MaxLength(DataConstants.BookLanguageMaxLength)]
        public string Language { get; set; }

        [Required]
        [MaxLength(DataConstants.BookDescriptionMaxLength)]
        public string Description { get; set; }

        //[Required]
        public IFormFile Image { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Цената трябва да бъде положително число.")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }
        //public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Publishers { get; set; }
    }
}
