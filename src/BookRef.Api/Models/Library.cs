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
        protected PersonalLibrary() {}
        public PersonalLibrary(User user)
        {
            User = user;
        }

        public virtual ICollection<BookRecommedation> BookRecommedations { get; private set; } = new List<BookRecommedation>();
        public virtual ICollection<PersonRecommedation> PersonRecommedations { get; private set; } = new List<PersonRecommedation>();
        public virtual ICollection<PersonalBooks> MyBooks { get; private set; } = new List<PersonalBooks>();


        public void AddBookRecommendation(Book sourceBook, Book recommendedBook, string note = "")
        {
            var rec = new BookRecommedation
            {
                PersonalLibraryId = this.Id,
                SourceBook = sourceBook,
                RecommendedBook = recommendedBook,
                Note = new Note
                {
                    Content = note
                }
            };
            BookRecommedations.Add(rec);
        }

        public void AddPersonRecommendation(Book sourceBook, Person recommendedPerson, string note = "")
        {
            var rec = new PersonRecommedation
            {
                PersonalLibraryId = this.Id,
                SourceBook = sourceBook,
                RecommendedPerson = recommendedPerson,
                Note = new Note
                {
                    Content = note
                }
            };
            PersonRecommedations.Add(rec);
        }

        // public override string ToString()
        // {
        //     var myBooks = string.Join("|", MyBooks.Select(e => e.Book.Title));
        //     var allMyRec = string.Join("|", MyRecommendations
        //                                         .Select(e => $"From ‘{e.SourceBook.Title}‘"));
        //     return $"Person {{ Id = {Id}, UserId = {UserId}, MyBooks = {myBooks}, MyRecommendations = {allMyRec} }}";
        // }

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
            //UserId = @event.UserId;
            Id = @event.LibraryId;
        }

        private void OnBookAded(BookAdded @event)
        {
            var ub = new PersonalBooks
            {
                BookId = @event.BookId
            };
            MyBooks.Add(ub);
        }


        public void AddNewBook(long bookId)
        {
            if (Version == -1)
            {
                throw new NotFoundException("No user Library found", null);
            }

            Apply(new BookAdded(Id, bookId));
        }

        public void AddNewBook(Book book)
        {
            if (Version == -1)
            {
                throw new NotFoundException("No user Library found", null);
            }

            Apply(new BookAdded(Id, book.Id));
        }

        // Only to seed data, remove in production
        public void AddBookDataSeeder(Book book)
        {
            var ub = new PersonalBooks
            {
                Book = book
            };
            MyBooks.Add(ub);
        }
    }
}
