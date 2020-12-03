using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class BookAuthor
    {
        protected BookAuthor() {}

        public BookAuthor(Book book, Author author)
        {
            Book = book;
            Author = author;
        }

        public BookAuthor(int bookId, int authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
        }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Author Author { get; set; }
        public long AuthorId { get; set; }
    }
}
