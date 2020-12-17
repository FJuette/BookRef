using System.Linq;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Framework
{
    public class Query
    {
        /// <summary>
        /// Gets all books.
        /// </summary>
        [UseFiltering]
        public IQueryable<UserBooks> GetBooks([Service]ILibraryService context)
        {
            var user = context.GetPersonalLibrary();
            return user.MyBooks.AsQueryable();
        }

        /// <summary>
        /// Gets all recommendations.
        /// </summary>
        [UseFiltering]
        public IQueryable<Recommedation> GetRecommedations([Service]ILibraryService context)
        {
            var user = context.GetPersonalLibrary();
            return user.MyRecommendations.AsQueryable();
        }

        /// <summary>
        /// Gets all authors.
        /// </summary>
        [UseProjection]
        [UseFiltering]
        public IQueryable<Author> GetAuthors([Service]BookRefDbContext context) =>
            context.Authors;

        /// <summary>
        /// Gets all categories.
        /// </summary>
        [UseFiltering]
        public IQueryable<Category> GetCategories([Service]BookRefDbContext context) =>
            context.Categories;

        /// <summary>
        /// Gets all People.
        /// </summary>
        [UseFiltering]
        public IQueryable<Person> GetPeople([Service]BookRefDbContext context) =>
            context.People;

        /// <summary>
        /// Gets all Speaker.
        /// </summary>
        [UseFiltering]
        public IQueryable<Speaker> GetSpeakers([Service]BookRefDbContext context) =>
            context.Speakers;

    }
}
