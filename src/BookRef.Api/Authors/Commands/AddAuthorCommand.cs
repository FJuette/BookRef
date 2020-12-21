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
    public record AddAuthorCommand(string Name);

    // Represents the output of our GraphQL mutation
    public class AddAuthorPayload
    {
        public AddAuthorPayload(Author author)
        {
            Author = author;
        }

        public Author Author { get; }
    }

    public class AddAuthorCommandHandler
    {
        [UseApplicationDbContext]
        public async Task<AddAuthorPayload> AddAuthorAsync(
             AddAuthorCommand input,
             [ScopedService] BookRefDbContext context)
        {
            var author = new Author(input.Name);
            context.Authors.Add(author);
            await context.SaveChangesAsync();

            return new AddAuthorPayload(author);
        }
    }

    public class AddAuthorValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
