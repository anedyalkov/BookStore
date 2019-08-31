using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web
{
    public class WebConstants
    {
        public const string AdministratorRole = "Администратор";
        public const string AdminArea = "Admin";

        public const string TempDataSuccessMessageKey = "SuccessMessage";
        public const string TempDataErrorMessageKey = "ErrorMessage";

        public const string BookNotFound = "Книгата не беше намерена.";
       
        public const string OrderError = "Поръчката не може да бъде завършена";

        public const string ReviewNotCreated = "Мнението не беше създадено.";

        public const string BookExistInCart = "Книгата вече е добавена в количката.";
        public const string BookNotRemovedFromCart = "Книгата не беше премахнта от количката.";

    }
}
