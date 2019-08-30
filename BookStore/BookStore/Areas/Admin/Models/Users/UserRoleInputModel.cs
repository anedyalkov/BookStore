using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.Areas.Admin.Models.Users
{
    public class UserRoleInputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
