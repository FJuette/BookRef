using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class BookCategory
    {
        public Book Book { get; set; }
        public long BookId { get; set; }

        public Category Category { get; set; }
        public long CategoryId { get; set; }

        public bool IsPrimary { get; set; }
    }
}
