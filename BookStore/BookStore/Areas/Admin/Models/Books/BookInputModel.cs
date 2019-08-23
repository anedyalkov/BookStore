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

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.BookTitleMaxLength)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [Display(Name = "Автор")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [Display(Name = "Издателство")]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.BookLanguageMaxLength)]
        [Display(Name = "Език")]
        public string Language { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [MaxLength(DataConstants.BookDescriptionMaxLength)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [Display(Name = "Снимка")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.ErrorMsg))]
        [Range(0.1, double.MaxValue, ErrorMessage = "Цената трябва да бъде положително число.")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата на създаване")]
        public DateTime? CreatedOn { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();
    }
}
