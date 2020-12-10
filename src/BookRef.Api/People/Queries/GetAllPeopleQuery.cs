using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.People.Queries
{
    public record GetAllPeopleQuery() : IRequest<PeopleViewModel>;
    public record PeopleViewModel(IEnumerable<Person> Data);

    public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, PeopleViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetAllPeopleQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PeopleViewModel> Handle(
            GetAllPeopleQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _ctx.People
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new PeopleViewModel(data);
        }
    }
}
