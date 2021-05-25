using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Persistence
{
#nullable disable
    public class BookRefDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookRecommedation> BookRecommedations { get; set; }
        public DbSet<PersonRecommedation> PersonRecommedations { get; set; }
        public DbSet<PersonalLibrary> Libraries { get; set; }
        public DbSet<PersonalBook> PersonalBooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public BookRefDbContext(DbContextOptions<BookRefDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            builder?.Entity<Author>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Name)
                    .IsRequired(true);
            });
            builder?.Entity<Category>(b =>
            {
                b.Property(e => e.Name)
                    .IsRequired(true);
            });
            builder?.Entity<Person>(b =>
            {
                b.Property(e => e.Name)
                    .IsRequired(true);
            });
            builder?.Entity<Speaker>(b =>
            {
                b.Property(e => e.Name)
                    .IsRequired(true);
            });
            builder?.Entity<Book>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(c => c.Language)
                    .HasConversion<string>();
                b.HasMany(e => e.Authors)
                    .WithMany(e => e.Books);
            });

            // Book to Catogory n...m with extra property
            builder?.Entity<BookCategory>(b =>
            {
                b.HasKey(e => new { e.CategoryId, e.BookId });
                b.HasOne(e => e.Book)
                    .WithMany(e => e.BookCategories)
                    .HasForeignKey(e => e.BookId);
                b.HasOne(e => e.Category)
                    .WithMany(e => e.BookCategories)
                    .HasForeignKey(e => e.CategoryId);
            });

            builder?.Entity<PersonalLibrary>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).ValueGeneratedNever();
                b.HasMany(e => e.BookRecommedations)
                    .WithOne()
                    .HasForeignKey(e => e.PersonalLibraryId);
                b.HasMany(e => e.PersonRecommedations)
                    .WithOne()
                    .HasForeignKey(e => e.PersonalLibraryId);
                b.HasMany(e => e.MyBooks)
                    .WithOne()
                    .HasForeignKey(e => e.PersonalLibraryId);
                b.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<User>(e => e.PersonalLibraryId);
            });

            builder?.Entity<PersonalBook>(b =>
            {
                b.HasKey(e => e.Id);
                b.HasOne(e => e.Book)
                    .WithMany()
                    .HasForeignKey(e => e.BookId);
                b.Property(c => c.Status)
                    .HasConversion<string>();
            });

            builder?.Entity<BaseRecommedation>(b =>
            {
                b.HasOne(e => e.Note)
                    .WithOne()
                    .HasForeignKey<BaseRecommedation>(e => e.NoteId);
            });

            // Personal User recommendations from a book
            builder?.Entity<BookRecommedation>(b =>
            {
                b.HasOne(e => e.SourceBook)
                    .WithMany()
                    .HasForeignKey(e => e.SourceBookId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(e => e.RecommendedBook)
                    .WithMany()
                    .HasForeignKey(e => e.RecommendedBookId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            builder?.Entity<PersonRecommedation>(b =>
            {
                b.HasOne(e => e.SourceBook)
                    .WithMany()
                    .HasForeignKey(e => e.SourceBookId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(e => e.RecommendedPerson)
                    .WithMany()
                    .HasForeignKey(e => e.RecommendedPersonId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // builder?.Entity<Friends>(b =>
            // {
            //     b.HasOne(e => e.SourceUser)
            //         .WithMany()
            //         .HasForeignKey(e => e.SourceUserId);
            //     b.HasOne(e => e.FriendUser)
            //         .WithMany()
            //         .HasForeignKey(e => e.FriendUserId);
            // });
        }

        public override Task<int> SaveChangesAsync(
                CancellationToken cancellationToken = new CancellationToken())
        {
            this.SetLastChangeDate();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            this.SetLastChangeDate();
            return base.SaveChanges();
        }

    }

    public static class ContextExtensions
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public static void SetLastChangeDate(
            this DbContext context)
        {
            foreach (var entityEntry in context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added))
            {
                if (entityEntry.Entity is PersonalBook entityToMark)
                {
                    entityToMark.LastChanged = DateTime.Now;
                }
            }
        }
    }
}
