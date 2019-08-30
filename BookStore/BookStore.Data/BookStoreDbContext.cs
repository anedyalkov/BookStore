using BookStore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class BookStoreDbContext: IdentityDbContext<BookStoreUser,IdentityRole,string>
    {
   
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options): base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<CategoryBook> CategoryBooks { get; set; }

        public DbSet<ShoppingCartBook> ShoppingCartBooks { get; set; }

        public DbSet<OrderBook> OrderBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CategoryBook>().HasKey(cb => new { cb.CategoryId, cb.BookId });
            modelBuilder.Entity<ShoppingCartBook>().HasKey(shb => new { shb.ShoppingCartId, shb.BookId });
            modelBuilder.Entity<OrderBook>().HasKey(ob => new { ob.OrderId, ob.BookId });

            modelBuilder.Entity<BookStoreUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<BookStoreUser>()
               .HasMany(u => u.Reviews)
               .WithOne(r => r.Creator)
               .HasForeignKey(r => r.CreatorId);

            modelBuilder.Entity<ShoppingCart>()
                   .HasOne(x => x.User)
                   .WithOne(x => x.ShoppingCart)
                   .HasForeignKey<BookStoreUser>(x => x.ShoppingCartId);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Book>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<CategoryBook>()
                .HasOne(cb => cb.Category)
                .WithMany(b => b.CategoryBooks)
                .HasForeignKey(cb => cb.CategoryId);

            modelBuilder.Entity<CategoryBook>()
                .HasOne(cb => cb.Book)
                .WithMany(b => b.CategoryBooks)
                .HasForeignKey(cb => cb.BookId);

            modelBuilder.Entity<ShoppingCartBook>()
                .HasOne(shb => shb.ShoppingCart)
                .WithMany(sh => sh.ShoppingCartBooks)
                .HasForeignKey(shb => shb.ShoppingCartId);

            modelBuilder.Entity<ShoppingCartBook>()
             .HasOne(shb => shb.Book)
             .WithMany(sh => sh.ShoppingCartBooks)
             .HasForeignKey(shb => shb.BookId);

            modelBuilder.Entity<OrderBook>()
              .HasOne(ob => ob.Order)
              .WithMany(o => o.OrderBooks)
              .HasForeignKey(ob => ob.OrderId);

            modelBuilder.Entity<OrderBook>()
              .HasOne(ob => ob.Book)
              .WithMany(o => o.OrderBooks)
              .HasForeignKey(ob => ob.BookId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
