using System;
using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Exceptions;
using BookRef.Api.Library.Events;
using BookRef.Api.Models.Framework;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class PersonalLibrary : Aggregate
    {
        public virtual User User { get; private set; }
        public long UserId { get; set; }
        // Public needed for EventStore AggregateRepository
        public PersonalLibrary() {}
        public PersonalLibrary(User user)
        {
            User = user;
        }
        public PersonalLibrary(Guid id, User user)
        {
            Id = id;
            User = user;
        }

        public virtual ICollection<BookRecommedation> BookRecommedations { get; private set; } = new List<BookRecommedation>();
        public virtual ICollection<PersonRecommedation> PersonRecommedations { get; private set; } = new List<PersonRecommedation>();
        public virtual ICollection<PersonalBook> MyBooks { get; private set; } = new List<PersonalBook>();


        public void AddBookRecommendation(Book sourceBook, Book recommendedBook, string note = "")
        {
            var rec = new BookRecommedation(recommendedBook)
            {
                PersonalLibraryId = this.Id,
                SourceBook = sourceBook,
                Note = new Note(note)
            };
            BookRecommedations.Add(rec);
        }

        public void AddPersonRecommendation(Book sourceBook, Person recommendedPerson, string note = "")
        {
            var rec = new PersonRecommedation(recommendedPerson)
            {
                PersonalLibraryId = this.Id,
                SourceBook = sourceBook,
                Note = new Note(note)
            };
            PersonRecommedations.Add(rec);
        }

        public PersonalBook ChangeBookStatus(long personalBookId, BookStatus newStatus)
        {
            var pb = MyBooks.First(e => e.Id == personalBookId);
            pb.Status = newStatus;
            return pb;
        }

        public PersonalBook ChangeColorCode(long personalBookId, string colorCode)
        {
            var pb = MyBooks.First(e => e.Id == personalBookId);
            pb.ColorCode = colorCode;
            return pb;
        }

        // Events
        protected override void When(object @event)
        {
            switch (@event)
            {
                case LibraryCreated x: OnCreated(x); break;
                case BookAdded x: OnBookAded(x); break;
            }
        }

        public void Create(Guid libraryId, long userId)
        {
            if (Version >= 0)
            {
                //throw new UserAlreadyCreatedException();
            }

            Apply(new LibraryCreated(libraryId, userId));
        }

        private void OnCreated(LibraryCreated @event)
        {
            UserId = @event.UserId;
            Id = @event.LibraryId;
        }

        private void OnBookAded(BookAdded @event)
        {
            var ub = new PersonalBook(@event.LibraryId, @event.Book, @event.Status)
            {
                ColorCode = @event.ColorCode
            };
            MyBooks.Add(ub);
        }

        public void AddNewBook(Book book, BookStatus status, string? colorCode)
        {
            // Enable when ES is used
            // if (Version == -1)
            // {
            //     throw new NotFoundException("No user Library found", null);
            // }

            Apply(new BookAdded(Id, book, status, colorCode));
        }

        // Only to seed data, remove in production
        public void AddBookDataSeeder(Book book)
        {
            var ub = new PersonalBook(this.Id, book, BookStatus.Active);
            MyBooks.Add(ub);
        }

        // Events:
        // ----

        // MyBookRemoved
        // BookDeleted
        // CategoryRemoved
        // AuthorRemoved

        // Exceptions:
        // ---
        // BookAlreadyExists (same Isbn)

    }
}
