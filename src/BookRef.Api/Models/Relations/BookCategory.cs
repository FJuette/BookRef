using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class BookCategory
    {
        protected BookCategory() {}

        public BookCategory(long bookId, long categoryId, bool isPrimary = false)
        {
            BookId = bookId;
            CategoryId = categoryId;
            IsPrimary = isPrimary;
        }

        public BookCategory(Book book, Category category, bool isPrimary = false)
        {
            Book = book;
            Category = category;
            IsPrimary = isPrimary;
        }

        public virtual Book Book { get; private set; }
        public long BookId { get; private set; }

        public virtual Category Category { get; private set; }
        public long CategoryId { get; private set; }

        public bool IsPrimary { get; private set; }
    }
}
