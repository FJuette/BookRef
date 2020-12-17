using System;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using MediatR;

namespace BookRef.Api.Library.Commands
{
    public record CreateLibraryCommand() : IRequest<Guid>
    {

    }

    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, Guid>
    {
        private readonly BookRefDbContext _context;
        private readonly AggregateRepository _repository;

        public CreateLibraryCommandHandler(
            BookRefDbContext context, AggregateRepository repository)
            {
                _context = context;
                _repository = repository;
            }

        public async Task<Guid> Handle(
            CreateLibraryCommand request,
            CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var aggregate = await _repository.LoadAsync<PersonalLibrary>(id);
            // TODO put real userID here
            aggregate.Create(id, 1);

            await _repository.SaveAsync(aggregate);

            var result = _context.Libraries.Attach(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
        }
    }
}
