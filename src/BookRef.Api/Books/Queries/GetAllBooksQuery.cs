using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Books.Queries
{
    public record GetAllBooksQuery() : IRequest<BooksViewModel>;
    public record BooksViewModel(IEnumerable<Book> Data);

    public class GetAllStoriesQueryHandler : IRequestHandler<GetAllBooksQuery, BooksViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetAllStoriesQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<BooksViewModel> Handle(
            GetAllBooksQuery request,
            CancellationToken cancellationToken)
        {
            var stories = await _ctx.Books
                .Include(e => e.BookAuthors)
                .Include(e => e.BookCategories)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new BooksViewModel(stories);
        }
    }
}
