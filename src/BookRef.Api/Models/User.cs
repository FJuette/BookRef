using System;
using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Authors.Commands;
using BookRef.Api.Models.Framework;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Users.Commands;
using BookRef.Api.Users.Events;

namespace BookRef.Api.Models
{
    public class User : Aggregate
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }

        private List<Recommedation> _myRecommendations = new List<Recommedation>();
        public IReadOnlyList<Recommedation> MyRecommendations => _myRecommendations.ToList();

        private List<UserBooks> _myBooks = new List<UserBooks>();
        public IReadOnlyList<UserBooks> MyBooks => _myBooks.ToList();

        public void AddNewBook(Book book, BookStatus status = BookStatus.Active)
        {
            var ub = new UserBooks
            {
                Book = book,
                User = this,
                Status = status
            };
            _myBooks.Add(ub);
        }

        public void AddBookRecommendation(Book sourceBook, Book recommendedBook, string note = "")
        {
            var rec = new Recommedation
            {
                Owner = this,
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
                Owner = this,
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
            return $"Person {{ Id = {Id}, Username = {Username}, Password = {Password}, EMail = {EMail}, MyBooks = {myBooks}, MyRecommendations = {allMyRec} }}";
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case UserCreated x: OnCreated(x); break;
            }
        }

        public void Create(Guid id, string username)
        {
            if (Version >= 0)
            {
                //throw new UserAlreadyCreatedException();
            }

            Apply(new UserCreated() { Id = id, Username = username });
        }

        private void OnCreated(UserCreated @event)
        {
            Username = @event.Username;
            Id = @event.Id;
        }
    }
}
