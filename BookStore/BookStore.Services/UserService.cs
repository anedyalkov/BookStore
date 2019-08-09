﻿using BookStore.Data;
using BookStore.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class UserService : IUserService
    {
        private readonly BookStoreDbContext db;
        private readonly UserManager<BookStoreUser> userManager;

        public UserService(BookStoreDbContext db, UserManager<BookStoreUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public async Task<BookStoreUser> GetByUsername(string username) 
        {
            return await this.userManager.FindByNameAsync(username);
        }
    }
}