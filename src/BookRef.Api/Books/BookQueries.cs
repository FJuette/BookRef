using System.Collections.Generic;
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

namespace BookRef.Api.Authors
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
    }
}
