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
        public long UserId { get; private set; }
        public PersonalLibrary() {}
        public PersonalLibrary(long userId)
        {
            UserId = userId;
        }

        private List<Recommedation> _myRecommendations = new List<Recommedation>();
        public IReadOnlyList<Recommedation> MyRecommendations => _myRecommendations.ToList();

        private List<UserBooks> _myBooks = new List<UserBooks>();
        public IReadOnlyList<UserBooks> MyBooks => _myBooks.ToList();


        public void AddBookRecommendation(Book sourceBook, Book recommendedBook, string note = "")
        {
            var rec = new Recommedation
            {
                OwnerId = this.Id,
                SourceBook = sourceBook,
                RecommendedBook = recommendedBook,
                Note = new Note
                {
                    Content = note
                }
            };
            _myRecommendations.Add(rec);
        }

        public void AddPersonRecommendation(Book sourceBook, Person recommendedPerson, string note = "")
        {
            var rec = new Recommedation
            {
                OwnerId = this.Id,
                SourceBook = sourceBook,
                RecommendedPerson = recommendedPerson,
                Note = new Note
                {
                    Content = note
                }
            };
            _myRecommendations.Add(rec);
        }

        public override string ToString()
        {
            var myBooks = string.Join("|", _myBooks.Select(e => e.Book.Title));
            var allMyRec = string.Join("|", _myRecommendations
                                                .Where(e => e.RecommendedBook != null)
                                                .Select(e => $"From ‘{e.SourceBook.Title}‘ for ‘{e.RecommendedBook.Title}‘ with note ‘{e.Note.Content}‘"));
            return $"Person {{ Id = {Id}, UserId = {UserId}, MyBooks = {myBooks}, MyRecommendations = {allMyRec} }}";
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
            var ub = new UserBooks
            {
                BookId = @event.BookId,
                UserId = this.UserId
            };
            _myBooks.Add(ub);
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
            var ub = new UserBooks
            {
                BookId = book.Id,
                UserId = this.UserId
            };
            _myBooks.Add(ub);
        }
    }
}
