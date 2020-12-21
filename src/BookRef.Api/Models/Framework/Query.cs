using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class Query
    {
        // [UseFiltering]
        // public IQueryable<Book> GetBooks([Service]BookRefDbContext context)
        // {
        //     return context.Books;
        // }

        // public Task<Book> GetBookAsync(
        //     long id,
        //     BooksByIdDataLoader dataLoader,
        //     CancellationToken cancellationToken)
        //     {
        //         return dataLoader.LoadAsync(id, cancellationToken);
        //     }

        // Cannot use PersonalLibrary directly -> HotChocolate problem with getting the types
        public IQueryable<PersonalBooks> GetLibrary([Service]BookRefDbContext context)
        {
            return context.PersonalBooks;
        }

        public IQueryable<BookRecommedation> GetBookRecommedations([Service]BookRefDbContext context)
        {
            return context.BookRecommedations;
        }

        public IQueryable<PersonRecommedation> GetPeopleRecommedations([Service]BookRefDbContext context)
        {
            return context.PersonRecommedations;
        }

        public Task<Author> GetAuthorAsync(
            long id,
            AuthorsByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            {
                return dataLoader.LoadAsync(id, cancellationToken);
            }

        public IQueryable<Author> GetAuthors(
            [Service]BookRefDbContext context)
            {
                return context.Authors;
            }

        public IQueryable<Category> GetCategories(
            [Service]BookRefDbContext context) => context.Categories;

        public IQueryable<Person> GetPeople(
            [Service]BookRefDbContext context) => context.People;

        public IQueryable<Speaker> GetSpeakers(
            [Service]BookRefDbContext context) => context.Speakers;

    }
}
