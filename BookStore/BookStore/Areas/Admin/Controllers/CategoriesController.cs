using BookStore.Services;
using BookStore.Services.Admin;
using BookStore.Services.Admin.Models.Categories;
using BookStore.Web.Areas.Admin.Models.Categories;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminController
    {
        private readonly IAdminCategoryService categoryService;

        public CategoriesController(IAdminCategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategories().ToListAsync();

            return this.View(categories);
        }

        public IActionResult Create()
        {
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryInputModel categoryModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryModel);
            }

            await this.categoryService.CreateAsync(categoryModel.Name);

            this.TempData.AddSuccessMessage(string.Format(
               WebAdminConstants.CategoryCreatedMsg,
               categoryModel.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoryService
                .GetByIdAsync<AdminCategoryDetailsServiceModel>(id);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            return this.View(new CategoryInputModel
            {
                Name = category.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryInputModel categoryModel)
        {

            if (!this.ModelState.IsValid)
            {
                return this.View(categoryModel);
            }

            await this.categoryService.EditAsync(
                id,
                categoryModel.Name);

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.CategoryUpdatedMsg,
                categoryModel.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var category = await this.categoryService
                .GetByIdAsync<AdminCategoryDetailsServiceModel>(id);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }

            await this.categoryService.Hide(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.CategoryHiddenMsg,
              category.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var category = await this.categoryService
                .GetByIdAsync<AdminCategoryDetailsServiceModel>(id);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFoundMsg);
                return RedirectToAction(nameof(Index));
            }
            await this.categoryService.Show(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.CategoryShowedMsg,
              category.Name));

            return this.RedirectToAction(nameof(Index));
        }
    }
}
