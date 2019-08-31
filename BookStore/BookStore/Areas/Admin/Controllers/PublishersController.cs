using BookStore.Services.Admin;
using BookStore.Web.Areas.Admin.Models.Publishers;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class PublishersController : AdminController
    {
        private readonly IAdminPublisherService publisherService;
        private readonly IAdminBookService bookService;

        public PublishersController(IAdminPublisherService publisherService, IAdminBookService bookService)
        {
            this.publisherService = publisherService;
            this.bookService = bookService;
        }
        public async Task<IActionResult> Index()
        {
            var publishers = await this.publisherService.GetAllPublishers().ToListAsync();

            return this.View(publishers);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PublisherInputModel publisherModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(publisherModel);
            }

            var result = await this.publisherService.CreateAsync(publisherModel.Name);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotCreated);
                return RedirectToAction(nameof(Index));
            }

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Books(int id)
        {
            var publisher = await this.publisherService.GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFound);
                return this.RedirectToAction(nameof(Index));
            }
            var books = bookService.GetBooksByPublisherId(id);
            return this.View(books);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var publisher = await this.publisherService
                .GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFound);
                return RedirectToAction(nameof(Index));
            }

            return this.View(new PublisherInputModel
            {
                Name = publisher.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PublisherInputModel publisherModel)
        {

            if (!this.ModelState.IsValid)
            {
                return this.View(publisherModel);
            }

            var result = await this.publisherService.EditAsync(
                id,
                publisherModel.Name);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotEdited);
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.PublisherUpdated,
                publisherModel.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var publisher = await this.publisherService
                .GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFound);
                return RedirectToAction(nameof(Index));
            }

            var result = await this.publisherService.HideAsync(id);

            if (result == false)
            {
                this.TempData.AddErrorMessage(string.Format(WebAdminConstants.PublisherIncludesBooks,
                    publisher.Name));
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.PublisherHidden,
              publisher.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var publisher = await this.publisherService
                .GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFound);
                return RedirectToAction(nameof(Index));
            }
            await this.publisherService.ShowAsync(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.PublisherShowed,
              publisher.Name));

            return this.RedirectToAction(nameof(Index));
        }
    }
}

