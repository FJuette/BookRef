using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Speakers.Queries
{
    public record GetAllSpeakerQuery() : IRequest<SpeakersViewModel>;
    public record SpeakersViewModel(IEnumerable<Speaker> Data);

    public class GetAllSpeakerQueryHandler : IRequestHandler<GetAllSpeakerQuery, SpeakersViewModel>
    {
        private readonly BookRefDbContext _ctx;

        public GetAllSpeakerQueryHandler(BookRefDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<SpeakersViewModel> Handle(
            GetAllSpeakerQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _ctx.Speakers
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new SpeakersViewModel(data);
        }
    }
}
