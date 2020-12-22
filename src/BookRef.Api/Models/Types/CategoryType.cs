using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class CategoryType : ObjectType<Category>
    {
        protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<CategoryByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(e => e.BookCategories)
                .ResolveWith<CategoryResolvers>(t => t.GetBooksAsync(default!, default!, default!, default))
                .UseDbContext<BookRefDbContext>()
                .Name("books");
        }
        private class CategoryResolvers
        {
            public async Task<IEnumerable<Book>> GetBooksAsync(
                Category category,
                [ScopedService] BookRefDbContext dbContext,
                BookByIdDataLoader bookById,
                CancellationToken cancellationToken)
            {
                long[] bookIds = await dbContext.Categories
                    .Where(s => s.Id == category.Id)
                    .SelectMany(s => s.BookCategories.Select(t => t.BookId))
                    .ToArrayAsync();

                return await bookById.LoadAsync(bookIds, cancellationToken);
            }
        }
    }
}
