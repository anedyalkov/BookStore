using BookStore.Domain;
using BookStore.Services.Admin.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Models.Users
{
    public class UserRolesViewModel
    {
        public BookStoreUser User { get; set; }
        public IEnumerable<SelectListItem> UserRoles { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
    }
}
