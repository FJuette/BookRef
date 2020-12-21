using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using FluentValidation;
using HotChocolate;
using MediatR;

namespace BookRef.Api.Authors.Commands
{
    public record AddAuthorCommand : IRequest<long>
    {
        public string Name { get; init; }
    }

    // public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, long>
    // {
    //     private readonly BookRefDbContext _context;


    //     public AddAuthorCommandHandler(
    //         [ScopedService] BookRefDbContext context) =>
    //         _context = context;

    //     public async Task<long> Handle(
    //         AddAuthorCommand request,
    //         CancellationToken cancellationToken)
    //     {
    //         var item = new Author(request.Name);

    //         // Prefer attach over add/update
    //         var result = _context.Authors.Attach(item);
    //         await _context.SaveChangesAsync(cancellationToken);
    //         return result.Entity.Id;
    //     }
    // }

    public class AddAuthorValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
