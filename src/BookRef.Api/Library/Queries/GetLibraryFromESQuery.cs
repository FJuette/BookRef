using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Library.Queries
{
    public record GetLibraryFromESQuery() : IRequest<PersonalLibrary>;


    public class GetLibraryFromESQueryHandler : IRequestHandler<GetLibraryFromESQuery, PersonalLibrary>
    {
        private readonly AggregateRepository _repository;
        private readonly BookRefDbContext _ctx;

        public GetLibraryFromESQueryHandler(BookRefDbContext ctx, AggregateRepository repository)
        {
            _repository = repository;
            _ctx = ctx;
        }

        public async Task<PersonalLibrary> Handle(
            GetLibraryFromESQuery request,
            CancellationToken cancellationToken)
        {
            var user = _ctx.Users.First(e => e.Id == 1);
            var aggregate = await _repository.LoadAsync<PersonalLibrary>(user.PersonalLibraryId);
            return aggregate;
        }
    }
}
