using System.Linq;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Queries
{
    public class Query
    {
        /// <summary>
        /// Gets all authors.
        /// </summary>
        //[UseSelection]
        public IQueryable<Author> GetAuthors([Service]BookRefDbContext context) =>
            context.Authors;

        /// <summary>
        /// Gets all books.
        /// </summary>
        [UseFiltering]
        public IQueryable<UserBooks> GetBooks([Service]IUserService context)
        {
            var user = context.GetCurrentUser();
            return user.MyBooks.AsQueryable();
        }
    }
}
