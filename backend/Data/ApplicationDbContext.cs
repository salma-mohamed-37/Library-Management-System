using backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Borrowed> Borrowed { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<Category>().Property(x => x.Name).IsRequired();

            builder.Entity<Author>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<Author>().Property(x => x.Name).IsRequired();

            builder.Entity<Book>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<Book>().Property(x => x.Name).IsRequired();
            builder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(b => b.Books)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Book>()
               .HasOne(b => b.Author)
               .WithMany(b => b.Books)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Borrowed>()
               .HasOne(b => b.Book)
               .WithMany(b => b.Borrowed)
               .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Borrowed>()
               .HasOne(b => b.User)
               .WithMany(b => b.Borrowed)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Borrowed>().HasKey(x => new { x.UserId, x.BookId, x.BorrowDate });
            
            base.OnModelCreating(builder);
        }
    }
}
