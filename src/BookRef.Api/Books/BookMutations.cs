using System.Linq;
using System.Threading.Tasks;
using BookRef.Api.Common;
using BookRef.Api.Extensions;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace BookRef.Api.Books
{
    public record AddBookInput(string? Isbn, string Title, string? Subtitle);
    public record MoveBookToLibraryInput([ID(nameof(Book))] long BookId, BookStatus Status, string? ColorCode);
    public record AddBookRecommendationInput([ID(nameof(Book))] long SourceBookId, [ID(nameof(Book))] long TargetBookId, string? Note);
    public record AddPersonRecommendationInput([ID(nameof(Book))] long SourceBookId, [ID(nameof(Person))] long TargetPersonId, string? Note);
    public record AddNewAuthorInput([ID(nameof(Book))] long BookId, string Name);
    public record AddExistingAuthorInput([ID(nameof(Book))] long BookId, [ID(nameof(Author))] long AuthorId);
    public record AddCategoryInput([ID(nameof(Book))] long BookId, [ID(nameof(Category))] long CategoryId, bool IsPrimary);
    public record MoveBookStatusInput([ID(nameof(PersonalBook))] long PersonalBookId, BookStatus NewStatus);
    public record ChangeColorCodeInput([ID(nameof(PersonalBook))] long PersonalBookId, string ColorCode);


    [ExtendObjectType(Name = "Mutation")]
    public class BookMutations
    {
        // BookAdded AND MyBookAdded
        [UseApplicationDbContext]
        public async Task<Payload<Book>> AddBookAsync(
             AddBookInput input,
             [Service] IGetClaimsProvider claimsProvider,
             [ScopedService] BookRefDbContext context)
        {
            var book = new Book(input.Isbn, input.Title, claimsProvider.Username)
            {
                Subtitle = input.Subtitle
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return new Payload<Book>(book);
        }

        // RecommendedBookMovedToReadingList
        [UseApplicationDbContext]
        public async Task<Payload<PersonalBook>> MoveBookInLibraryAsync(
             MoveBookToLibraryInput input,
             [ScopedService] BookRefDbContext context,
             [Service] IGetClaimsProvider claimsProvider)
        {
            var library = context.Libraries.First(e => e.Id == claimsProvider.LibraryId);
            var book = context.Books.Find(input.BookId);
            library.AddNewBook(book, input.Status, input.ColorCode);
            await context.SaveChangesAsync();

            return new Payload<PersonalBook>(library.MyBooks.Last());
        }

        // RecommendationAdded
        [UseApplicationDbContext]
        public async Task<Payload<BookRecommedation>> AddBookRecommendationAsync(
             AddBookRecommendationInput input,
             [ScopedService] BookRefDbContext context,
             [Service] IGetClaimsProvider claimsProvider)
        {
            var library = context.Libraries.First(e => e.Id == claimsProvider.LibraryId);
            var source = context.Books.First(e => e.Id == input.SourceBookId);
            var target = context.Books.First(e => e.Id == input.TargetBookId);
            library.AddBookRecommendation(source, target, input.Note != null ? input.Note : "");
            await context.SaveChangesAsync();
            return new Payload<BookRecommedation>(library.BookRecommedations.Last());
        }

        // RecommendationAdded
        [UseApplicationDbContext]
        public async Task<Payload<PersonRecommedation>> AddPersonRecommendationAsync(
             AddPersonRecommendationInput input,
             [ScopedService] BookRefDbContext context,
             [Service] IGetClaimsProvider claimsProvider)
        {
            var library = context.Libraries.First(e => e.Id == claimsProvider.LibraryId);
            var source = context.Books.First(e => e.Id == input.SourceBookId);
            var target = context.People.First(e => e.Id == input.TargetPersonId);
            library.AddPersonRecommendation(source, target, input.Note != null ? input.Note : "");
            await context.SaveChangesAsync();
            return new Payload<PersonRecommedation>(library.PersonRecommedations.Last());
        }

        // NewAuthorAdded
        [UseApplicationDbContext]
        public async Task<Payload<Book>> AddNewAuthorAsync(
             AddNewAuthorInput input,
             [ScopedService] BookRefDbContext context)
        {
            var book = context.Books.Find(input.BookId);
            var author = new Author(input.Name);
            context.Authors.Add(author);
            book.AddAuthor(author);

            await context.SaveChangesAsync();
            return new Payload<Book>(book);
        }

        // AuthorAdded
        [UseApplicationDbContext]
        public async Task<Payload<Book>> AddAuthorAsync(
             AddExistingAuthorInput input,
             [ScopedService] BookRefDbContext context)
        {
            var book = context.Books.Find(input.BookId);
            var author = context.Authors.Find(input.AuthorId);
            book.AddAuthor(author);

            await context.SaveChangesAsync();
            return new Payload<Book>(book);
        }

        // CategoryAdded
        [UseApplicationDbContext]
        public async Task<Payload<Book>> AddCategoryAsync(
             AddCategoryInput input,
             [ScopedService] BookRefDbContext context)
        {
            var book = context.Books.Find(input.BookId);
            var category = context.Categories.Find(input.CategoryId);
            book.AddCategory(category, input.IsPrimary);

            await context.SaveChangesAsync();
            return new Payload<Book>(book);
        }

        // BookStatusMoved (e.g. from wish to active)
        [UseApplicationDbContext]
        public async Task<Payload<PersonalBook>> ChangeBookStatusAsync(
             MoveBookStatusInput input,
             [ScopedService] BookRefDbContext context,
             [Service] IGetClaimsProvider claimsProvider)
        {
            var library = context.Libraries.First(e => e.Id == claimsProvider.LibraryId);
            var pb = library.ChangeBookStatus(input.PersonalBookId, input.NewStatus);

            await context.SaveChangesAsync();
            return new Payload<PersonalBook>(pb);
        }

        [UseApplicationDbContext]
        public async Task<Payload<PersonalBook>> ChangeColorCodeAsync(
             ChangeColorCodeInput input,
             [ScopedService] BookRefDbContext context,
             [Service] IGetClaimsProvider claimsProvider)
        {
            var library = context.Libraries.First(e => e.Id == claimsProvider.LibraryId);
            var pb = library.ChangeColorCode(input.PersonalBookId, input.ColorCode);

            await context.SaveChangesAsync();
            return new Payload<PersonalBook>(pb);
        }

        // [UseApplicationDbContext]
        // public async Task<AddBookPayload> AddNewBookAsync(
        //      AddNewBookInput input,
        //      [ScopedService] BookRefDbContext context,
        //      [Service] IOpenLibraryService service)
        // {
        //     // TODO API call to get all data for this book
        //     var book = service.FindBook(input.Isbn);
        //     book.Language = BookLanguage.German;
        //     context.Books.Add(book);
        //     await context.SaveChangesAsync();
        //     return new AddBookPayload(book);
        // }
    }
}
