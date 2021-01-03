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
    public record AddBookToLibraryInput(string Isbn, BookStatus Status);
    public record AddNewBookInput(string Isbn, string? Title, string? Auflage, BookLanguage Language = BookLanguage.German);
    public record AddNewBookDraftInput(string Name);

    [ExtendObjectType(Name = "Mutation")]
    public class BookMutations
    {
        [UseApplicationDbContext]
        public async Task<AddBookToLibrary> AddBookToLibraryAsync(
             AddBookToLibraryInput input,
             [ScopedService] BookRefDbContext context)
        {
            var book = context.Books.FirstOrDefault(e => e.Isbn == input.Isbn);
            if (book == null)
                return new AddBookToLibrary(new List<UserError>{ new UserError("ISBN not found", "4001") });

            var library = context.Libraries.First();
            library.AddNewBook(book, input.Status);
            await context.SaveChangesAsync();
            return new AddBookToLibrary(library.MyBooks.Last());
        }

        [UseApplicationDbContext]
        public async Task<AddBookPayload> AddNewBookAsync(
             AddNewBookInput input,
             [ScopedService] BookRefDbContext context)
        {
            // TODO API call to get all data for this book
            var book = new Book(input.Isbn, input.Title != null ? input.Title : "Missing Title");
            book.Language = input.Language;
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return new AddBookPayload(book);
        }
    }

    public class AddBookToLibrary : Payload
    {
        public AddBookToLibrary(PersonalBook personalBook)
        {
            PersonalBook = personalBook;
        }
        public AddBookToLibrary(IReadOnlyList<UserError> errors)
             : base(errors)
        {
        }

        public PersonalBook PersonalBook { get; }
    }

    public class AddBookPayload : Payload
    {
        public AddBookPayload(Book book)
        {
            Book = book;
        }
        public AddBookPayload(IReadOnlyList<UserError> errors)
             : base(errors)
        {
        }

        public Book Book { get; }
    }
}
