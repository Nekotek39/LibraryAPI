using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Copy> Copy { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(a => a.books)
                .WithOne(b => b.author)
                .HasForeignKey(b => b.authorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.copies)
                .WithOne(c => c.book)
                .HasForeignKey(c => c.bookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
