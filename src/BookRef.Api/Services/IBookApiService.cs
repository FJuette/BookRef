using BookRef.Api.Models;
using LanguageExt;

namespace BookRef.Api.Services
{
    public interface IBookApiService
    {
        Option<Book> FindBook(string isbn);
    }
}
