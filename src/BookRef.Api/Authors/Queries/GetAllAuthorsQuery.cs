using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Authors.Queries
{
    public record GetAllAuthorsQuery() : IRequest<AuthorsViewModel>;
    public record AuthorsViewModel(IEnumerable<Author> Data);

    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, AuthorsViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetAllAuthorsQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<AuthorsViewModel> Handle(
            GetAllAuthorsQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _ctx.Authors
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new AuthorsViewModel(data);
        }
    }
}
