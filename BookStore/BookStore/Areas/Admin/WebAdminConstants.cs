using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin
{
    public class WebAdminConstants
    {
        public const string UserAddedToRole = "Потребител ({0}) беше добавен към роля {1}.";
        public const string UserInvalidIdentityDetails = "Невалидни детайли на потребителя.";
        public const string UserRemovedFromRole = "Потребител ({0}) беше премахнат от роля {1}.";

        public const string CategoryCreated = "Категория ({0}) беше създадена успешно.";
        public const string CategoryNotCreated = "Категорията не беше създадена.";
        public const string CategoryNotEdited = "Категорията не беше редактирана.";
        public const string CategoryNotFound = "Не сте избрали категория.";
        public const string CategoryUpdated = "Категория ({0}) беше успешно редактирана.";
        public const string CategoryHidden = "Категория ({0}) беше успешнo скрита.";
        public const string CategoryShowed = "Категория ({0}) беше успешнo разкрита.";
        public const string CategoryAddedToBook = "Категория ({0}) беше успешно добавена към книга ({1})";
        public const string CategoryAlreadyAddedToBook = "Категория ({0}) вече е добавена към книга ({1})";
        public const string CategoryRemovedFromBook = "Категория ({0}) беше успешно премахната от книга ({1})";
        public const string CategoryIncludesBooks = "Категория ({0}) не може да бъде скрита,защото притежава книги.";
        

        public const string PublisherCreated = "Издателство ({0}) беше създадена успешно.";
        public const string PublisherNotCreated = "Издателството не беше създадено.";
        public const string PublisherNotEdited = "Издателството не беше редактирано.";
        public const string PublisherNotFound = "Издателство ({0}) не беше намеренo.";
        public const string PublisherUpdated = "Издателство ({0}) беше успешно редактирано.";
        public const string PublisherHidden = "Издателство ({0}) беше успешнo скритo.";
        public const string PublisherShowed = "Издателство ({0}) беше успешнo разкритo.";
        public const string PublisherIncludesBooks = "Издателство ({0}) не може да бъде скрито,защото притежава книги.";

        public const string AuthorCreated = "Автор ({0} {1}) беше създаден успешно.";
        public const string AuthorNotCreated = "Авторът не беше създаден.";
        public const string AuthorNotEdited = "Авторът не беше редактиран.";
        public const string AuthorNotFound = "Автор ({0} {1}) не беше намерен.";
        public const string AuthorUpdated = "Автор ({0} {1}) беше успешно редактиран.";
        public const string AuthorHidden = "Автор ({0} {1}) беше успешнo скрит.";
        public const string AuthorShowed = "Автор ({0} {1}) беше успешнo разкрит.";
        public const string AuthorIncludesBooks = "Автор ({0}) не може да бъде скрит,защото притежава книги.";

        public const string BookCreated = "Книга ({0}) беше създадена успешно. Моля добави категория!";
        public const string BookNotCreated = "Книга не беше създадена.";
        public const string BookNotEdited = "Книга не беше редактирана.";
        public const string BookNotFound = "Книга ({0}) не беше намерена.";
        public const string BookUpdated = "Книга ({0}) беше успешно редактирана.";
        public const string BookHidden = "Книга ({0}) беше успешнo скрита.";
        public const string BookShowed = "Книга ({0}) беше успешнo разкрита.";

        public const string Error = "Полето ({0}) e задължително.";


        public const string ReviewDeleted = "Мнението беше изтрито успешно.";




    }
}
