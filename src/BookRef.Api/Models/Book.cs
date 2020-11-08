using System;
using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models.Relations;

namespace BookRef.Api.Models
{
    public class Book : EntityBase
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public byte[] Image { get; set; }
        public string Auflage { get; set; }
        public DateTime Created { get; set; }

        public Language Language { get; set; }

        public User Creator { get; set; }

        private List<BookCategory> _bookCategories = new List<BookCategory>();
        public IReadOnlyList<BookCategory> BookCategories => _bookCategories.ToList();

        private List<BookAuthor> _bookAuthors = new List<BookAuthor>();
        public IReadOnlyList<BookAuthor> BookAuthors => _bookAuthors.ToList();

    }

    public enum Language
    {
        German,
        English
    }
}
