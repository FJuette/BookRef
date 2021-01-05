using System;

namespace BookRef.Api.Models.Relations
{
    public class PersonalBook
    {
        protected PersonalBook() {}
        public PersonalBook(Guid personalLibraryId, Book book, BookStatus bookStatus)
        {
            PersonalLibraryId = personalLibraryId;
            Book = book;
            Status = bookStatus;
        }
        public long Id { get; set; }
        public DateTime? StartDate { get; set; }
        public int CurrentPage { get; set; }

        public BookStatus Status { get; set; } //! Start
        public Guid PersonalLibraryId { get; set; }

        public virtual Book Book { get; set; }
        public long BookId { get; set; }
        public string? ColorCode { get; set; } //! Start
        public DateTime LastChanged { get; set; } = DateTime.Now;

        //public Speaker? Speaker { get; set; }
    }

    public enum BookStatus
    {
        Active,
        Done,
        Wish
    }
}
