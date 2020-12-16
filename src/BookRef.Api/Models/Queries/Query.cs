using System.Linq;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using HotChocolate;
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

        // public IQueryable<User> GetUsers([Service]BookRefDbContext context) =>
        //     context.Users;
    }
}
