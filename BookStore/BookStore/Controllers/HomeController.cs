﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;
using BookStore.Web.Models.Home;
using BookStore.Services.Models.Publishers;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;
        private readonly IPublisherService publisherService;

        public HomeController(IBookService bookService, ICategoryService categoryService, IPublisherService publisherService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
            this.publisherService = publisherService;
        }
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    var books = await this.bookService.GetAllActiveBooks().ToListAsync();

            //    return this.View(books);
            //}

            if (this.User.Identity.IsAuthenticated)
            {
                var books = await this.bookService.GetBooksFilter(model.CategoryId);
                var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();

                model.Books = books.ToList();
                model.Categories = categories;

                return this.View(model);
            }

            return this.View();
        }

        public async Task<IActionResult> Search(string searchText)
        {
            var categories = await this.categoryService.GetAllActiveCategories().ToListAsync();
            var viewModel = new SearchViewModel
            {
                SearchText = /*searchModel.SearchText*/searchText
            };

            var books = await this.bookService.FindBooks(/*searchModel.SearchText*/searchText).ToListAsync();
            //viewModel.Books = viewModel.Books.Concat(books).ToList();
            //var publisher = await publisherService.GetPublisherBySerchText<PublisherBasicServiceModel>(searchModel.SearchText);
            //if (publisher != null)
            //{
            //    var publisherBooks = publisher.Books;
            //    viewModel.Books = viewModel.Books.Concat(publisherBooks).ToList();
            //}
            viewModel.Books = books;
            viewModel.Categories = categories;


            return View(viewModel);
        }
    }
}
