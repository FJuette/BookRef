using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Infrastructure;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Books.Queries
{
    public record GetAllActiveBooksQuery() : IRequest<ActiveBooksViewModel>;
    public record ActiveBooksViewModel(IEnumerable<dynamic> Data);

    public class GetAllActiveBooksQueryHandler : IRequestHandler<GetAllActiveBooksQuery, ActiveBooksViewModel>
    {
        private readonly ILibraryService _userService;

        public GetAllActiveBooksQueryHandler(ILibraryService userService)
        {
            _userService = userService;
        }

        public async Task<ActiveBooksViewModel> Handle(
            GetAllActiveBooksQuery request,
            CancellationToken cancellationToken)
        {
            var user = _userService.GetPersonalLibrary();
            var books = user.MyBooks.Where(e => e.Status == Models.Relations.BookStatus.Active)
                            .Select(e => new { e.BookId, e.Book.Title, authors = e.Book.GetAuthors(), e.CurrentPage, e.Speaker?.Name, e.Type } );

            return new ActiveBooksViewModel(books);
        }
    }
}
