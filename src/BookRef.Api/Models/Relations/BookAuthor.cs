using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class BookAuthor
    {
        public Book Book { get; set; }
        public long BookId { get; set; }

        public Author Author { get; set; }
        public long AuthorId { get; set; }
    }
}
