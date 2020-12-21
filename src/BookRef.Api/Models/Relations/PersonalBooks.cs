using System;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class PersonalBooks
    {
        public long PersonalBooksId { get; set; }
        public DateTime? StartDate { get; set; }
        public int CurrentPage { get; set; }

        public BookStatus Status { get; set; }
        public BookType Type { get; set; }
        //public virtual PersonalLibrary PersonalLibrary { get; set; }
        public Guid PersonalLibraryId { get; set; }

        public virtual Book Book { get; set; }
        public long BookId { get; set; }

        //public Speaker? Speaker { get; set; }
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
