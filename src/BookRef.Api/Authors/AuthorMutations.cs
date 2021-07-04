// using System.Collections.Generic;
// using System.Threading.Tasks;
// using BookRef.Api.Common;
// using BookRef.Api.Extensions;
// using BookRef.Api.Models.ValueObjects;
// using BookRef.Api.Persistence;
// using FluentValidation;
// using HotChocolate;
// using HotChocolate.Types;

// namespace BookRef.Api.Authors
// {
//     public record AddAuthorInput(string Name);

//     [ExtendObjectType(Name = "Mutation")]
//     public class AuthorMutations
//     {
//         [UseApplicationDbContext]
//         public async Task<AddAuthorPayload> AddAuthorAsync(
//              AddAuthorInput input,
//              [ScopedService] BookRefDbContext context)
//         {
//             if (input.Name.Length < 5)
//             {
//                 return new AddAuthorPayload(
//                     new List<UserError>
//                     {
//                         new UserError("Minimum Length greater 5 required.", "AUTHOR LENGTH")
//                     });
//             }

//             var author = new Author(input.Name);
//             context.Authors.Add(author);
//             await context.SaveChangesAsync();

//             return new AddAuthorPayload(author);
//         }
//     }

//     public class AddAuthorPayload : Payload<Author>
//     {
//         public AddAuthorPayload(Author author)
//         {
//             Author = author;
//         }
//         public AddAuthorPayload(IReadOnlyList<UserError> errors)
//              : base(errors)
//         {
//         }

//         public Author? Author { get; }
//     }
// }



// public record AddPersonInput(string Name);

//     [ExtendObjectType(Name = "Mutation")]
//     public class PeopleMutations
//     {
//         [UseApplicationDbContext]
//         public async Task<Payload<Person>> AddPersonAsync(
//              AddPersonInput input,
//              [ScopedService] BookRefDbContext context)
//         {
//             var person = new Person(input.Name);
//             context.People.Add(person);
//             await context.SaveChangesAsync();

//             return new Payload<Person>(person);
//         }
//     }
