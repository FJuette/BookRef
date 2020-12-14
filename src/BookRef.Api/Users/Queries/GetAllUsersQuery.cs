using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Users.Queries
{
    public record GetAllUsersQuery() : IRequest<UsersViewModel>;
    public record UsersViewModel(IEnumerable<User> Data);

    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllUsersQuery, UsersViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetAllAuthorsQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<UsersViewModel> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _ctx.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new UsersViewModel(data);
        }
    }
}
