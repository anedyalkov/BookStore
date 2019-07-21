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
        public const string CategoryHiddenMsg = "Категория ({0}) беше успешнo скрита.";
        public const string CategoryShowedMsg = "Категория ({0}) беше успешнo разкрита.";

        public const string PublisherCreatedMsg = "Издателство ({0}) беше създадена успешно.";
        public const string PublisherNotFoundMsg = "Издателство ({0}) не беше намеренo.";
        public const string PublisherUpdatedMsg = "Издателство ({0}) беше успешно редактирана.";
        public const string PublisherHiddenMsg = "Издателство ({0}) беше успешнo скритo.";
        public const string PublisherShowedMsg = "Издателство ({0}) беше успешнo разкритo.";
    }
}
