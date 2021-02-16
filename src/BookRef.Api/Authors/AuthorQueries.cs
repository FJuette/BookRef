using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Authors
{
    [ExtendObjectType(Name = "Query")]
    public class AuthorQueries
    {
        [UseApplicationDbContext]
        [UseFiltering(typeof(AuthorFilterInputType))]
        public Task<List<Author>> GetAuthorsAsync(
            [ScopedService] BookRefDbContext context) =>
             context.Authors.ToListAsync();

        public Task<Author> GetAuthorByIdAsync(
            [ID(nameof(Author))] long id,
            AuthorByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public Task<IReadOnlyList<Author>> GetAuthorsByIdAsync(
            [ID(nameof(Author))] long[] ids,
            AuthorByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}
