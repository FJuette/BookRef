using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using MediatR;

namespace BookRef.Api.Library.Commands
{
    public record AddBookToLibraryCommand : IRequest<bool>
    {
        public long BookId { get; init; }
    }

    // public class AddBookToLibraryCommandHandler : IRequestHandler<AddBookToLibraryCommand, bool>
    // {
    //     private readonly BookRefDbContext _context;
    //     private readonly AggregateRepository _repository;

    //     public AddBookToLibraryCommandHandler(
    //         BookRefDbContext context, AggregateRepository repository)
    //         {
    //             _context = context;
    //             _repository = repository;
    //         }

    //     public async Task<bool> Handle(
    //         AddBookToLibraryCommand request,
    //         CancellationToken cancellationToken)
    //     {
    //         var user = _context.Users.First(e => e.Id == 1);
    //         var aggregate = await _repository.LoadAsync<PersonalLibrary>(user.Library.Id);
    //         // TODO put real userID here
    //         aggregate.AddNewBook(request.BookId);

    //         await _repository.SaveAsync(aggregate);

    //         var result = _context.Libraries.Attach(aggregate);
    //         await _context.SaveChangesAsync(cancellationToken);
    //         return true;
    //     }
    // }
}
