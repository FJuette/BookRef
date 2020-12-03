using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class BookCategory
    {
        protected BookCategory() {}

        public BookCategory(Book book, Category category)
        {
            Book = book;
            Category = category;
        }

        public BookCategory(int bookId, int categoryId)
        {
            BookId = bookId;
            CategoryId = categoryId;
        }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Category Category { get; set; }
        public long CategoryId { get; set; }

        public bool IsPrimary { get; set; }
    }
}
