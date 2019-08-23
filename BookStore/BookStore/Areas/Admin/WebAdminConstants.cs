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
        public const string CategoryNotFoundMsg = "Не сте избрали категория.";
        public const string CategoryUpdatedMsg = "Категория ({0}) беше успешно редактирана.";
        public const string CategoryHiddenMsg = "Категория ({0}) беше успешнo скрита.";
        public const string CategoryShowedMsg = "Категория ({0}) беше успешнo разкрита.";
        public const string CategoryAddedToBookMsg = "Категория ({0}) беше успешно добавена към книга ({1})";
        public const string CategoryAlreadyAddedToBookMsg = "Категория ({0}) вече е добавена към книга ({1})";
        public const string CategoryRemovedFromBookMsg = "Категория ({0}) беше успешно премахната от книга ({1})";
        public const string CategoryIncludesBooksMsg = "Категория ({0}) не може да бъде скрита,защото притежава книги.";
        

        public const string PublisherCreatedMsg = "Издателство ({0}) беше създадена успешно.";
        public const string PublisherNotFoundMsg = "Издателство ({0}) не беше намеренo.";
        public const string PublisherUpdatedMsg = "Издателство ({0}) беше успешно редактирано.";
        public const string PublisherHiddenMsg = "Издателство ({0}) беше успешнo скритo.";
        public const string PublisherShowedMsg = "Издателство ({0}) беше успешнo разкритo.";
        public const string PublisherIncludesBooksMsg= "Издателство ({0}) не може да бъде скрито,защото притежава книги.";

        public const string AuthorCreatedMsg = "Автор ({0} {1}) беше създаден успешно.";
        public const string AuthorNotFoundMsg = "Автор ({0} {1}) не беше намерен.";
        public const string AuthorUpdatedMsg = "Автор ({0} {1}) беше успешно редактиран.";
        public const string AuthorHiddenMsg = "Автор ({0} {1}) беше успешнo скрит.";
        public const string AuthorShowedMsg = "Автор ({0} {1}) беше успешнo разкрит.";
        public const string AuthorIncludesBooksMsg = "Автор ({0}) не може да бъде скрит,защото притежава книги.";

        public const string BookCreatedMsg = "Книга ({0}) беше създадена успешно. Моля добави категория!";
        public const string BookNotFoundMsg = "Книга ({0}) не беше намерена.";
        public const string BookUpdatedMsg = "Книга ({0}) беше успешно редактирана.";
        public const string BookHiddenMsg = "Книга ({0}) беше успешнo скрита.";
        public const string BookShowedMsg = "Книга ({0}) беше успешнo разкрита.";

        public const string ErrorMsg = "Полето ({0}) e задължително.";


        public const string ReviewDeletedMsg = "Мнението беше изтрито успешно.";




    }
}
