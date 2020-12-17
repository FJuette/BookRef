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
    public record GetWishlistQuery() : IRequest<WishlistViewModel>;
    public record WishlistViewModel(IEnumerable<dynamic> Data);

    public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, WishlistViewModel>
    {
        private readonly ILibraryService _userService;

        public GetWishlistQueryHandler(ILibraryService userService)
        {
            _userService = userService;
        }

        public async Task<WishlistViewModel> Handle(
            GetWishlistQuery request,
            CancellationToken cancellationToken)
        {
            var user = _userService.GetPersonalLibrary();
            var books = user.MyBooks.Where(e => e.Status == Models.Relations.BookStatus.Wish)
                            .Select(e => new { e.BookId, e.Book.Title, authors = e.Book.GetAuthors(), e.CurrentPage, e.Speaker?.Name, e.Type } );

            return new WishlistViewModel(books);
        }
    }
}
