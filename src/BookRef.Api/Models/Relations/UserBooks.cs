using System;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class UserBooks
    {
        public long UserBooksId { get; set; }
        public DateTime? StartDate { get; set; }
        public int CurrentPage { get; set; }

        public BookStatus Status { get; set; }
        public BookType Type { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

        public Book Book { get; set; }
        public long BookId { get; set; }

        public Speaker? Speaker { get; set; }

    }

    public enum BookStatus
    {
        Active,
        Done,
        Wish
    }

    public enum BookType
    {
        Book,
        AudioBook,
        EBook
    }
}
