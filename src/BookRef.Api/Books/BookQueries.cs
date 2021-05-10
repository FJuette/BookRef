using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.Types;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Books
{
    [ExtendObjectType(Name = "Query")]
    public class BookQueries
    {
        [UseApplicationDbContext]
        [UsePaging(typeof(NonNullType<BookType>))]
        [UseFiltering(typeof(BookFilterInputType))]
        [UseSorting]
        public IQueryable<Book> GetAllBooks(
            [ScopedService] BookRefDbContext context) =>
             context.Books;

        public Task<Book> GetBookByIdAsync(
            [ID(nameof(Book))] long id,
            BookByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public Task<IReadOnlyList<Book>> GetBooksByIdAsync(
            [ID(nameof(Book))] long[] ids,
            BookByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);

        // Personal books
        [UseApplicationDbContext]
        [UseFiltering(typeof(PersonalBookFilterInputType))]
        //[UseSorting]
        public Task<IReadOnlyList<PersonalBook>> GetBooksAsync(
            PersonalBookByIdDataLoader dataLoader,
            [Service] IGetClaimsProvider claimsProvider,
            [ScopedService] BookRefDbContext context,
            CancellationToken cancellationToken)
            {
                var ids = context.Libraries
                    .Include(e => e.MyBooks)
                    .First(e => e.Id == claimsProvider.LibraryId)
                    .MyBooks.Select(e => e.Id);
                return dataLoader.LoadAsync(ids.ToArray(), cancellationToken);
            }

        [UseApplicationDbContext]
        public Task<List<BookRecommedation>> GetAllBookRecommendations(
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
        public async Task<MyRecommendations> GetRecommendationsForBook(
            [ID(nameof(Book))] long id,
            BookByIdDataLoader dataLoader,
            [Service] IGetClaimsProvider claimsProvider,
            [ScopedService] BookRefDbContext context,
            CancellationToken cancellationToken)
            {
                var library = context.Libraries
                    .First(e => e.Id == claimsProvider.LibraryId);
                var result = new MyRecommendations();
                var source = await dataLoader.LoadAsync(id, cancellationToken);
                result.SourceBook = source;
                result.BookRecommedations = library.BookRecommedations.Where(e => e.SourceBookId == source.Id).ToList();
                result.PersonRecommedations = library.PersonRecommedations.Where(e => e.SourceBookId == source.Id).ToList();
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
