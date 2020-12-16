using System.Linq;
using BookRef.Api.Exceptions;
using BookRef.Api.Models;
using BookRef.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Infrastructure
{
    public interface IUserService
    {
        User GetCurrentUser();
    }

    public class UserService : IUserService
    {
        private readonly BookRefDbContext _context;
        private readonly IGetClaimsProvider _provider;

        public UserService(BookRefDbContext context, IGetClaimsProvider provider)
        {
            _context = context;
            _provider = provider;
        }
        public User GetCurrentUser()
        {
            var user = _context.Users
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
                            .FirstOrDefault(e => e.Username == _provider.UserId);
            return user != null ? user : throw new BadUserException(_provider.UserId);
        }
    }
}
