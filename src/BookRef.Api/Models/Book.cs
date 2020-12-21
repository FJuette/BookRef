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
        public string Auflage { get; set; }
        public DateTime Created { get; set; }

        public BookLanguage Language { get; set; }

        //public User Creator { get; set; }

        public virtual ICollection<BookCategory> BookCategories { get; private set; } = new List<BookCategory>();

        public virtual ICollection<Author> Authors { get; private set; } = new List<Author>();

        public void SetAuthors(IEnumerable<Author> authors)
        {
            Authors = authors.ToList();
        }

        public void SetCategories(IEnumerable<Category> categories)
        {
            BookCategories = categories.Select(e => new BookCategory(this, e)).ToList();
        }


        public override string ToString()
        {
            var authors = string.Join("|", Authors.Select(e => e.Name));
            var categories = string.Join('|', BookCategories.Select(e => e.Category.Name));
            return $"Book {{ Id = {Id}, ISBN = {Isbn}, Title = {Title}, Auflage = {Auflage}, Language = {Language}, Authors = {authors}, Categories = {categories} }}";
        }
    }

    public enum BookLanguage
    {
        German,
        English
    }
}
