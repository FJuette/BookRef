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
    public record BookRecViewModel(MyRecsDto Data);

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
                .Include(e => e.RecommendedPerson)
                .Include(e => e.Note)
                .Where(e => e.SourceBookId == request.SourceBook)
                .ToListAsync(cancellationToken);

            var books = data.Where(e => e.RecommendedBook != null)
                            .Select(e => new MyBookRecDto { Book = e.RecommendedBook, Note = e.Note.Content } )
                            .ToList();
            var people = data.Where(e => e.RecommendedPerson != null)
                             .Select(e => new MyPersonRecDto { Person = e.RecommendedPerson, Note = e.Note.Content } )
                             .ToList();

            var myRecs = new MyRecsDto
            {
                Books = books,
                People = people
            };

            return new BookRecViewModel(myRecs);
        }
    }


    public record MyBookRecDto()
    {
        public Book Book { get; init; }
        public string Note { get; init; }
    }
    public record MyPersonRecDto()
    {
        public Person Person { get; init; }
        public string Note { get; init; }
    }

    public record MyRecsDto()
    {
        public List<MyBookRecDto> Books { get; init; }
        public List<MyPersonRecDto> People { get; init; }
    }

}
