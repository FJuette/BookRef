using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models.Relations;

namespace BookRef.Api.Models
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }

        private List<Recommedation> _myRecommendations = new List<Recommedation>();
        public IReadOnlyList<Recommedation> MyRecommendations => _myRecommendations.ToList();

        private List<UserBooks> _myBooks = new List<UserBooks>();
        public IReadOnlyList<UserBooks> MyBooks => _myBooks.ToList();

        public void AddNewBook(Book book)
        {
            var ub = new UserBooks
            {
                Book = book,
                User = this
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

        public override string ToString()
        {
            var myBooks = string.Join("|", _myBooks.Select(e => e.Book.Title));
            var allMyRec = string.Join("|", _myRecommendations.Select(e => $"From ‘{e.SourceBook.Title}‘ for ‘{e.RecommendedBook.Title}‘ with note ‘{e.Note.Content}‘"));
            return $"Person {{ Id = {Id}, Username = {Username}, Password = {Password}, EMail = {EMail}, MyBooks = {myBooks}, MyRecommendations = {allMyRec} }}";
        }
    }
}
