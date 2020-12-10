using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Recommandations.Queries
{
    public record GetRecByBookQuery(long SourceBook) : IRequest<BookRecViewModel>;
    public record BookRecViewModel(IEnumerable<MyRecDto> Data);

    public class GetRecByBookQueryHandler : IRequestHandler<GetRecByBookQuery, BookRecViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetRecByBookQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<BookRecViewModel> Handle(
            GetRecByBookQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _ctx.Recommedations
                .AsNoTracking()
                .Include(e => e.RecommendedBook)
                .Include(e => e.SourceBook)
                .Where(e => e.SourceBookId == request.SourceBook)
                .ToListAsync(cancellationToken);

            var books = data.Select(e => new MyRecDto
            {
                Book = e.RecommendedBook
            }).ToList();

            return new BookRecViewModel(books);
        }
    }

    public record MyRecDto()
    {
        public Book Book { get; init; }
    }
}
