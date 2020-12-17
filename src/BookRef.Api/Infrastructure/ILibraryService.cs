using System.Linq;
using BookRef.Api.Exceptions;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Infrastructure
{
    public interface ILibraryService
    {
        PersonalLibrary GetPersonalLibrary();
    }

    public class LibraryService : ILibraryService
    {
        private readonly BookRefDbContext _context;
        private readonly IGetClaimsProvider _provider;

        public LibraryService(BookRefDbContext context, IGetClaimsProvider provider)
        {
            _context = context;
            _provider = provider;
        }
        public PersonalLibrary GetPersonalLibrary()
        {
            var library = _context.Libraries
                            .Include(e => e.MyBooks)
                                .ThenInclude(e => e.Book)
                                .ThenInclude(e => e.BookAuthors)
                                .ThenInclude(e => e.Author)
                            .Include(e => e.MyRecommendations)
                                .ThenInclude(e => e.RecommendedBook)
                            .Include(e => e.MyRecommendations)
                                .ThenInclude(e => e.RecommendedPerson)
                            .Include(e => e.MyBooks)
                                .ThenInclude(e => e.Book)
                                .ThenInclude(e => e.BookCategories)
                                .ThenInclude(e => e.Category)
                            .FirstOrDefault(e => e.UserId == _provider.UserId);
            return library != null ? library : throw new BadUserException(_provider.UserId.ToString());
        }
    }
}
