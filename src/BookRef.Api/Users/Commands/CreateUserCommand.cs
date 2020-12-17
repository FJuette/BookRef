using System;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using MediatR;

namespace BookRef.Api.Users.Commands
{
    public record CreateUserCommand : IRequest<Guid>
    {
        public string Username { get; init; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly BookRefDbContext _context;
        private readonly AggregateRepository _repository;

        public CreateUserCommandHandler(
            BookRefDbContext context, AggregateRepository repository)
            {
                _context = context;
                _repository = repository;
            }

        public async Task<Guid> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            // var id = Guid.NewGuid();
            // var aggregate = await _repository.LoadAsync<User>(id);
            // aggregate.Create(id, request.Username);

            // await _repository.SaveAsync(aggregate);
            // var item = new User() { Id = id, Username = request.Username };

            // var result = _context.Users.Attach(item);
            // await _context.SaveChangesAsync(cancellationToken);
            // return result.Entity.Id;
            // return id;
            return Guid.NewGuid();
        }
    }

    // public class AddAuthorValidator : AbstractValidator<AddAuthorCommand>
    // {
    //     public AddAuthorValidator()
    //     {
    //         RuleFor(x => x.Name).NotEmpty()
    //             .WithMessage("Name is required");
    //     }
    // }
}
