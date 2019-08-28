using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Publishers;
using BookStore.Web.Areas.Admin.Models.Publishers;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class PublishersController : AdminController
    {
        private readonly IAdminPublisherService publisherService;

        public PublishersController(IAdminPublisherService publisherService)
        {
            this.publisherService = publisherService;
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
                // TODO errormessage
            }

            //this.TempData.AddSuccessMessage(string.Format(
            //   WebAdminConstants.PublisherCreatedMsg,
            //   publisherModel.Name));

            return this.RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var publisher = await this.publisherService
                .GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFoundMsg);
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
                // TODO errormessage
            }

            //this.TempData.AddSuccessMessage(string.Format(
            //    WebAdminConstants.PublisherUpdatedMsg,
            //    publisherModel.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var publisher = await this.publisherService
                .GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            var result = await this.publisherService.HideAsync(id);

            if (result == false)
            {
                this.TempData.AddErrorMessage(string.Format(WebAdminConstants.PublisherIncludesBooksMsg,
                    publisher.Name));
                return RedirectToAction(nameof(Index));
            }

            //this.TempData.AddSuccessMessage(string.Format(
            //  WebAdminConstants.PublisherHiddenMsg,
            //  publisher.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var publisher = await this.publisherService
                .GetByIdAsync(id);

            if (publisher == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.PublisherNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }
            await this.publisherService.ShowAsync(id);

            //this.TempData.AddSuccessMessage(string.Format(
            //  WebAdminConstants.PublisherShowedMsg,
            //  publisher.Name));

            return this.RedirectToAction(nameof(Index));
        }
    }
}

