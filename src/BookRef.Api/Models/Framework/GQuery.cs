using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Framework
{
    public class GQuery
    {
        [UseApplicationDbContext]
        public Task<List<Book>> GetBooks([ScopedService] BookRefDbContext context)
        {
            return context.Books.Include(e => e.Authors).ToListAsync();
        }

        public Task<Book> GetBookAsync(
            long id,
            BookByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            {
                return dataLoader.LoadAsync(id, cancellationToken);
            }

        // Cannot use PersonalLibrary directly -> HotChocolate problem with getting the types
        [UseApplicationDbContext]
        public Task<List<PersonalBooks>> GetLibrary([ScopedService] BookRefDbContext context)
        {
            return context.PersonalBooks.Include(e => e.Book).ToListAsync();
        }

        [UseApplicationDbContext]
        public Task<List<BookRecommedation>> GetBookRecommedations([ScopedService] BookRefDbContext context)
        {
            return context.BookRecommedations.ToListAsync();
        }

        [UseApplicationDbContext]
        public Task<List<PersonRecommedation>> GetPeopleRecommedations([ScopedService] BookRefDbContext context)
        {
            return context.PersonRecommedations.ToListAsync();
        }

        public Task<Author> GetAuthorAsync(
            long id,
            AuthorByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            {
                return dataLoader.LoadAsync(id, cancellationToken);
            }

        [UseApplicationDbContext]
        public Task<List<Author>> GetAuthors(
            [ScopedService] BookRefDbContext context)
            {
                return context.Authors.ToListAsync();
            }

        [UseApplicationDbContext]
        public Task<List<Category>> GetCategories(
            [ScopedService] BookRefDbContext context) => context.Categories.ToListAsync();

        [UseApplicationDbContext]
        public Task<List<Person>> GetPeople(
            [ScopedService] BookRefDbContext context) => context.People.ToListAsync();

        [UseApplicationDbContext]
        public Task<List<Speaker>> GetSpeakers(
            [ScopedService] BookRefDbContext context) => context.Speakers.ToListAsync();

    }
}
