using DocumentFormat.OpenXml.Bibliography;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Book>().ToTable("books");
        }

        internal Task GetBook(string title, Author author)
        {
            throw new NotImplementedException();
        }

        internal Task GetBook(string author, Title title)
        {
            throw new NotImplementedException();
        }

        internal Task Search(string title, DocumentFormat.OpenXml.Bibliography.Author author)
        {
            throw new NotImplementedException();
        }
    }
}
