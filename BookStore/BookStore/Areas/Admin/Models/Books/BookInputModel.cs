using BookStore.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.Areas.Admin.Models.Books
{
    public class BookInputModel
    {

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.BookTitleMaxLength)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [Display(Name = "Автор")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [Display(Name = "Издателство")]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.BookLanguageMaxLength)]
        [Display(Name = "Език")]
        public string Language { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.BookDescriptionMaxLength)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [Display(Name = "Снимка")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [Range(0.1, double.MaxValue, ErrorMessage = "Цената трябва да бъде положително число.")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата на създаване")]
        public DateTime? CreatedOn { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }
        public IEnumerable<SelectListItem> Publishers { get; set; }
    }
}
