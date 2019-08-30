using BookStore.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore.Web.Areas.Admin.Models.Users
{
    public class UserRolesViewModel
    {
        public BookStoreUser User { get; set; }

        public IEnumerable<SelectListItem> UserRoles { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
