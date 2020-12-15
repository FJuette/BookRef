using System;
using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class Book : EntityBase
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        //public byte[] Image { get; set; }
        public string Auflage { get; set; }
        public DateTime Created { get; set; }

        public Language Language { get; set; }

        public User Creator { get; set; }

        private List<BookCategory> _bookCategories = new List<BookCategory>();
        public IReadOnlyList<BookCategory> BookCategories => _bookCategories.ToList();

        private List<BookAuthor> _bookAuthors = new List<BookAuthor>();
        public IReadOnlyList<BookAuthor> BookAuthors => _bookAuthors.ToList();

        public void SetAuthors(IEnumerable<Author> authors)
        {
            _bookAuthors = authors.Select(e => new BookAuthor(this, e)).ToList();
        }

        public void SetCategories(IEnumerable<Category> categories)
        {
            _bookCategories = categories.Select(e => new BookCategory(this, e)).ToList();
        }

        public List<string> GetAuthors()
        {
            return _bookAuthors.Where(e => e.Author.Name != null).Select(e => e.Author.Name).ToList();
        }

        public override string ToString()
        {
            var authors = string.Join("|", _bookAuthors.Select(e => e.Author.Name));
            var categories = string.Join('|', _bookCategories.Select(e => e.Category.Name));
            return $"Book {{ Id = {Id}, ISBN = {Isbn}, Title = {Title}, Auflage = {Auflage}, Language = {Language}, Creator = {Creator}, Authors = {authors}, Categories = {categories} }}";
        }
    }

    public enum Language
    {
        German,
        English
    }
}
