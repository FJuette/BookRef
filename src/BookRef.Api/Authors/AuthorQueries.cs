using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Authors
{
    [ExtendObjectType(Name = "Query")]
    public class AuthorQueries
    {
        [UseApplicationDbContext]
        public Task<List<Author>> GetAuthors([ScopedService] BookRefDbContext context) =>
             context.Authors.ToListAsync();

        public Task<Author> GetAuthorAsync(
            [ID(nameof(Author))] long id,
            AuthorByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}
