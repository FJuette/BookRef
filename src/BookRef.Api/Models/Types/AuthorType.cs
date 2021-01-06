using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class AuthorType : ObjectType<Author>
    {
        protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<AuthorByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(e => e.Books)
                .ResolveWith<AuthorResolvers>(t => t.GetBooksAsync(default!, default!, default!, default))
                .UseDbContext<BookRefDbContext>()
                .Name("books");

            // Example UpperCase
            // descriptor.Field(e => e.Name)
            //     .UseUpperCase();
        }

        private class AuthorResolvers
        {
            public async Task<IEnumerable<Book>> GetBooksAsync(
                Author author,
                [ScopedService] BookRefDbContext dbContext,
                BookByIdDataLoader bookById,
                CancellationToken cancellationToken)
            {
                long[] bookIds = await dbContext.Books
                    .Where(s => s.Id == author.Id)
                    .Select(s => s.Id)
                    .ToArrayAsync();

                return await bookById.LoadAsync(bookIds, cancellationToken);
            }
        }
    }


}
