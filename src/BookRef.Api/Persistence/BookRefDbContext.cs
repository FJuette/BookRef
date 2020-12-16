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
        private static readonly Type[] _enumerationTypes = {}; // typeof()
        private readonly IWebHostEnvironment _env;
        private readonly string _userId;

        public BookRefDbContext(
            IWebHostEnvironment env,
            IGetClaimsProvider userData)
        {
            _env = env;
            _userId = userData?.UserId;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Recommedation> Recommedations { get; set; }
        public DbSet<User> Users { get; set; }
        // public DbSet<BookAuthor> BookAuthors { get; set; }
        // public DbSet<BookCategory> BookCategories { get; set; }
        // public DbSet<UserBooks> UserBooks { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Speaker> Speakers { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (_env.IsProduction())
            {
                optionsBuilder.UseSqlServer(EnvFactory.GetConnectionString());
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase(new Guid().ToString());
                optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            builder?.Entity<Author>(b =>
            {
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
                // b.HasOne(e => e.Creator)
                //     .WithMany();
            });

            // Book to Author n...m
            builder?.Entity<BookAuthor>(b =>
            {
                b.HasKey(e => new { e.AuthorId, e.BookId });
                b.HasOne(e => e.Book)
                    .WithMany(e => e.BookAuthors)
                    .HasForeignKey(e => e.BookId);
                b.HasOne(e => e.Author)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId);
            });

            // Book to Catogory n...m
            builder?.Entity<BookCategory>(b =>
            {
                b.HasKey(e => new { e.CategoryId, e.BookId });
                b.HasOne(e => e.Book)
                    .WithMany(e => e.BookCategories)
                    .HasForeignKey(e => e.BookId);
                b.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId);
            });

            // Book to User
            builder?.Entity<UserBooks>(b =>
            {
                b.HasKey(e => new { e.BookId, e.UserId } );
                b.HasOne<User>()
                    .WithMany(e => e.MyBooks)
                    .HasForeignKey(e => e.UserId);
                b.HasOne(e => e.Book)
                    .WithMany()
                    .HasForeignKey(e => e.BookId);
                b.Property(c => c.Status)
                    .HasConversion<string>();
                b.Property(c => c.Type)
                    .HasConversion<string>();
            });

            // Personal User recommendations from a book
            builder?.Entity<Recommedation>(b =>
            {
                b.HasOne(e => e.SourceBook)
                    .WithMany()
                    .HasForeignKey(e => e.SourceBookId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(e => e.RecommendedBook)
                    .WithMany()
                    .HasForeignKey(e => e.RecommendedBookId)
                    .OnDelete(DeleteBehavior.NoAction);
                b.HasOne(e => e.RecommendedPerson)
                    .WithMany()
                    .HasForeignKey(e => e.RecommendedPersonId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder?.Entity<User>(b =>
            {
                b.HasKey(e => e.Id);
                //b.Property(e => e.Id)
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
            MarkEnumTypesAsUnchanged();
            this.MarkCreatedItemAsOwnedBy(_userId);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            MarkEnumTypesAsUnchanged();
            this.MarkCreatedItemAsOwnedBy(_userId);
            return base.SaveChanges();
        }

        private void MarkEnumTypesAsUnchanged()
        {
            var enumerationEntries =
                ChangeTracker.Entries().Where(x => _enumerationTypes.Contains(x.Entity.GetType()));

            foreach (var enumerationEntry in enumerationEntries)
            {
                enumerationEntry.State = EntityState.Unchanged;
            }
        }
    }

    public static class ContextExtensions
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public static void MarkCreatedItemAsOwnedBy(
            this DbContext context,
            string userId)
        {
            foreach (var entityEntry in context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added))
            {
                if (entityEntry.Entity is IOwnedBy entityToMark)
                {
                    entityToMark.SetOwnedBy(userId);
                }
            }
        }
    }
}
