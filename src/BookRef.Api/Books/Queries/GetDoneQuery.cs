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
    public record GetDoneQuery() : IRequest<DoneViewModel>;
    public record DoneViewModel(IEnumerable<dynamic> Data);

    public class GetDoneQueryHandler : IRequestHandler<GetDoneQuery, DoneViewModel>
    {
        private readonly ILibraryService _userService;

        public GetDoneQueryHandler(ILibraryService userService)
        {
            _userService = userService;
        }

        public async Task<DoneViewModel> Handle(
            GetDoneQuery request,
            CancellationToken cancellationToken)
        {
            var user = _userService.GetPersonalLibrary();
            var books = user.MyBooks.Where(e => e.Status == Models.Relations.BookStatus.Done)
                            .Select(e => new { e.BookId, e.Book.Title, authors = e.Book.GetAuthors(), e.CurrentPage, e.Speaker?.Name, e.Type } );

            return new DoneViewModel(books);
        }
    }
}
