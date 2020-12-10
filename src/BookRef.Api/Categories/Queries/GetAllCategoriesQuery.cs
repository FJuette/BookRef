using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Categories.Queries
{
    public record GetAllCategoriesQuery() : IRequest<CategoriesViewModel>;
    public record CategoriesViewModel(IEnumerable<Category> Data);

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, CategoriesViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetAllCategoriesQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<CategoriesViewModel> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _ctx.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new CategoriesViewModel(data);
        }
    }
}
