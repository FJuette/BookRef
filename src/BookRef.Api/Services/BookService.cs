using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Services
{
    public interface IBookService
    {
        Option<Book> MatchAuthors(List<Author> authors, Option<Book> book);
    }

    public class BookService : IBookService
    {
        private List<Author> _authors;

        public Option<Book> MatchAuthors(List<Author> authors, Option<Book> book)
        {
            _authors = authors;
            book.IfSome(e => {
                e.SetAuthors(e.Authors.Select(FindExistingAuthor));
            });
            return book;
        }

        public Author FindExistingAuthor(Author author)
        {
            return _authors
                .FirstOrDefault(e => e.Name == author.Name)
                ?? author;
        }
    }
}
