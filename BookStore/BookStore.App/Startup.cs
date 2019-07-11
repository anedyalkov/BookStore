using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStore.App
{
    public class Program
    {
        public static void Main()
        {
            using (var db = new BookStoreDbContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                db.Database.Migrate();

                Seed(db);
            };
        }

        private static void Seed(BookStoreDbContext db)
        {
            var users = new User[]
                {
                    new User
                    {
                        Username = "pesho",
                        Password = "123",
                        Email = "pesho@abv.bg",
                        ShoppingCart = new ShoppingCart()
                    },
                    new User
                    {
                        Username = "gosho",
                        Password = "123",
                        Email = "gosho@abv.bg",
                        ShoppingCart = new ShoppingCart()
                    },
                    new User
                    {
                        Username = "niki",
                        Password = "123",
                        Email = "niki@abv.bg",
                        ShoppingCart = new ShoppingCart()
                    }
                };

            db.Users.AddRange(users);
            db.SaveChanges();
        }
    }
}
