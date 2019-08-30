using BookStore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.Areas.Admin.Models.Publishers
{
    public class PublisherInputModel
    {
        [Required(ErrorMessage = (WebAdminConstants.Error))]
        [MaxLength(DataConstants.PublisherNameMaxLength)]
        [Display(Name = "Име")]

        public string Name { get; set; }
    }
}
