namespace BookRef.Api.Models
{
    public class BookRecommedation : BaseRecommedation
    {
        protected BookRecommedation()
        {
            this.Type = RecommedationType.Book;
        }
        public BookRecommedation(Book book)
        {
            RecommendedBook = book;
        }
        public virtual Book RecommendedBook { get; private set; }
        public long RecommendedBookId { get; private set; }

    }
}
