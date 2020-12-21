using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor.Field(e => e.BookCategories)
                .ResolveWith<BookResolvers>(t => t.GetCategoriesAsync(default!, default!, default!, default))
                .UseDbContext<BookRefDbContext>()
                .Name("categories");
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
        }
    }


}
