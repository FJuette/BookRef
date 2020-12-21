using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor.Field(e => e.BookCategories)
                .ResolveWith<BookResolvers>(t => t.GetBooksAsync(default!, default!, default!, default))
                .UseDbContext<BookRefDbContext>()
                .Name("books");
        }

        private class BookResolvers
        {
            public async Task<IEnumerable<Book>> GetBooksAsync(
                Book book,
                BookRefDbContext dbContext,
                BooksByIdDataLoader bookById,
                CancellationToken cancellationToken)
            {
                long[] bookIds = await dbContext.Books
                    .Where(s => s.Id == book.Id)
                    .SelectMany(s => s.BookCategories.Select(t => t.BookId))
                    .ToArrayAsync();

                return await bookById.LoadAsync(bookIds, cancellationToken);
            }
        }
    }


}
