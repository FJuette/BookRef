using System.Collections.Generic;
using System.Threading.Tasks;
using BookRef.Api.Common;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using FluentValidation;
using HotChocolate;
using HotChocolate.Types;

namespace BookRef.Api.Authors
{
    // public record AddBookInput(string Name);

    // [ExtendObjectType(Name = "Mutation")]
    // public class AuthorMutations
    // {
    //     [UseApplicationDbContext]
    //     public async Task<AddAuthorPayload> AddAuthorAsync(
    //          AddAuthorInput input,
    //          [ScopedService] BookRefDbContext context)
    //     {
    //         var author = new Author(input.Name);
    //         context.Authors.Add(author);
    //         await context.SaveChangesAsync();

    //         return new AddAuthorPayload(author);
    //     }
    // }

    // public class AddAuthorPayload : AuthorPayloadBase
    // {
    //     public AddAuthorPayload(Author author) : base(author)
    //     {
    //         Author = author;
    //     }
    //     public AddAuthorPayload(IReadOnlyList<UserError> errors)
    //          : base(errors)
    //      {
    //      }

    //     public Author Author { get; }
    // }
}
