using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRef.Api.Common;
using BookRef.Api.Extensions;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using FluentValidation;
using HotChocolate;
using HotChocolate.Types;

namespace BookRef.Api.Books
{
    public record AddBookInput(string Isbn, BookStatus Status, BookFormat Format);

    [ExtendObjectType(Name = "Mutation")]
    public class BookMutations
    {
        [UseApplicationDbContext]
        public async Task<AddBookPayload> AddBookAsync(
             AddBookInput input,
             [ScopedService] BookRefDbContext context)
        {
            var book = context.Books.FirstOrDefault(e => e.Isbn == input.Isbn);
            var library = context.Libraries.First();
            library.AddNewBook(book, input.Status, input.Format);
            await context.SaveChangesAsync();
            return new AddBookPayload(library.MyBooks.Last());
            // var author = new Author(input.Name);
            // context.Authors.Add(author);
            // await context.SaveChangesAsync();

            // return new AddAuthorPayload(author);
        }
    }

    public class AddBookPayload : Payload
    {
        public AddBookPayload(PersonalBook personalBook)
        {
            PersonalBook = personalBook;
        }
        public AddBookPayload(IReadOnlyList<UserError> errors)
             : base(errors)
        {
        }

        public PersonalBook PersonalBook { get; }
    }
}
