using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin
{
    public class WebAdminConstants
    {
        public const string UserAddedToRoleMsg = "Потребител ({0}) беше добавен към роля {1}.";
        public const string UserInvalidIdentityDetailsMsg = "Невалидни Identity детайли.";
        public const string UserRemovedFromRoleMsg = "Потребител ({0}) беше премахнат от роля {1}.";

        public const string CategoryCreatedMsg = "Категория ({0}) беше създадена успешно.";
        public const string CategoryNotFoundMsg = "Категория ({0}) не беше намерена.";
        public const string CategoryUpdatedMsg = "Категория ({0}) беше успешно редактирана.";
        public const string CategoryHideMsg = "Категория ({0}) беше успешнo скрита.";
        public const string CategoryShowMsg = "Категория ({0}) беше успешнo разкрита.";
    }
}
