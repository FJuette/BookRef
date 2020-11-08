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
    }
}
