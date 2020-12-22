using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Books
{
    [ExtendObjectType(Name = "Query")]
    public class BookQueries
    {
        [UseApplicationDbContext]
        public Task<List<Book>> GetBooks([ScopedService] BookRefDbContext context) =>
             context.Books.ToListAsync();

        public Task<Book> GetBookAsync(
            [ID(nameof(Book))] long id,
            BookByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        [UseApplicationDbContext]
        public Task<List<BookRecommedation>> GetBookRecommendations(
            [ScopedService] BookRefDbContext context) =>
             context.BookRecommedations.ToListAsync();

        [UseApplicationDbContext]
        public Task<List<BookRecommedation>> GetBookRecommendationsForBook(
            [ID(nameof(Book))] long id,
            [ScopedService] BookRefDbContext context) =>
            context.BookRecommedations.Where(e => e.SourceBookId == id).ToListAsync();

        [UseApplicationDbContext]
        public Task<List<PersonRecommedation>> GetPeopleRecommendations(
            [ScopedService] BookRefDbContext context) =>
             context.PersonRecommedations.ToListAsync();

        [UseApplicationDbContext]
        public Task<List<PersonRecommedation>> GetPeopleRecommendationsForBook(
            [ID(nameof(Book))] long id,
            [ScopedService] BookRefDbContext context) =>
            context.PersonRecommedations.Where(e => e.SourceBookId == id).ToListAsync();

        [UseApplicationDbContext]
        public async Task<MyRecommendations> GetMyRecommendationsForBook(
            [ID(nameof(Book))] long id,
            BookByIdDataLoader dataLoader,
            [ScopedService] BookRefDbContext context,
            CancellationToken cancellationToken)
            {
                var result = new MyRecommendations();
                result.SourceBook = await dataLoader.LoadAsync(id, cancellationToken);
                result.BookRecommedations = await context.BookRecommedations.Where(e => e.SourceBookId == id).ToListAsync();
                result.PersonRecommedations = await context.PersonRecommedations.Where(e => e.SourceBookId == id).ToListAsync();
                return result;
            }

    }
}

public class MyRecommendations
{
    public Book SourceBook { get; set; }
    public List<BookRecommedation> BookRecommedations { get; set; }
    public List<PersonRecommedation> PersonRecommedations { get; set; }
}