using BookStore.Services.Admin;
using BookStore.Web.Areas.Admin.Models.Categories;
using BookStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var result = await this.categoryService.CreateAsync(categoryModel.Name);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotCreated);
                return RedirectToAction(nameof(Index));
            }

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoryService
                .GetByIdAsync(id);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFound);
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

            var result = await this.categoryService.EditAsync(
                id,
                categoryModel.Name);

            if (result == false)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotEdited);
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
                WebAdminConstants.CategoryUpdated,
                categoryModel.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var category = await this.categoryService
                .GetByIdAsync(id);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFound);
                return RedirectToAction(nameof(Index));
            }

           var result = await this.categoryService.HideAsync(id);

            if (result == false)
            {
                this.TempData.AddErrorMessage(string.Format(WebAdminConstants.CategoryIncludesBooks,
                    category.Name));
                return RedirectToAction(nameof(Index));
            }

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.CategoryHidden,
              category.Name));

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var category = await this.categoryService
                .GetByIdAsync(id);

            if (category == null)
            {
                this.TempData.AddErrorMessage(WebAdminConstants.CategoryNotFound);
                return RedirectToAction(nameof(Index));
            }
            await this.categoryService.ShowAsync(id);

            this.TempData.AddSuccessMessage(string.Format(
              WebAdminConstants.CategoryShowed,
              category.Name));

            return this.RedirectToAction(nameof(Index));
        }
    }
}
