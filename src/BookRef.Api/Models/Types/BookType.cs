using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<BookByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(e => e.BookCategories)
                .ResolveWith<BookResolvers>(t => t.GetCategoriesAsync(default!, default!, default!, default))
                .UseDbContext<BookRefDbContext>()
                .Name("categories");

            descriptor.Field(e => e.Authors)
                .ResolveWith<BookResolvers>(t => t.GetAuthorsAsync(default!, default!, default!, default))
                .UseDbContext<BookRefDbContext>()
                .Name("authors");
        }

        private class BookResolvers
        {
            public async Task<IEnumerable<Category>> GetCategoriesAsync(
                Book book,
                [ScopedService] BookRefDbContext dbContext,
                CategoryByIdDataLoader catById,
                CancellationToken cancellationToken)
            {
                long[] catIds = await dbContext.Books
                    .Where(s => s.Id == book.Id)
                    .SelectMany(s => s.BookCategories.Select(t => t.BookId))
                    .ToArrayAsync();

                return await catById.LoadAsync(catIds, cancellationToken);
            }

            public async Task<IEnumerable<Author>> GetAuthorsAsync(
                Book book,
                [ScopedService] BookRefDbContext dbContext,
                AuthorByIdDataLoader authorById,
                CancellationToken cancellationToken)
            {
                long[] authorIds = await dbContext.Books
                    .Where(s => s.Id == book.Id)
                    .SelectMany(s => s.Authors.Select(t => t.Id))
                    .ToArrayAsync();

                return await authorById.LoadAsync(authorIds, cancellationToken);
            }
        }
    }


}
