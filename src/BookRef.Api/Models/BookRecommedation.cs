using System;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class BookRecommedation : BaseRecommedation
    {
        public BookRecommedation()
        {
            this.Type = RecommedationType.Book;
        }
        public virtual Book? RecommendedBook { get; set; }
        public long? RecommendedBookId { get; set; }

    }
}
