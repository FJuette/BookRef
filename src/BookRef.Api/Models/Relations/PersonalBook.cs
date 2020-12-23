using System;
using System.Text.Json.Serialization;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models.Relations
{
    public class PersonalBook
    {
        protected PersonalBook() {}
        public PersonalBook(Guid personalLibraryId, Book book, BookStatus bookStatus, BookFormat bookFormat)
        {
            PersonalLibraryId = personalLibraryId;
            Book = book;
            Status = bookStatus;
            Format = bookFormat;
        }
        public long Id { get; set; }
        public DateTime? StartDate { get; set; }
        public int CurrentPage { get; set; }

        public BookStatus Status { get; set; }
        public BookFormat Format { get; set; }
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

    public enum BookFormat
    {
        Book,
        AudioBook,
        EBook
    }
}
