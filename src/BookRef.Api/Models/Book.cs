using System;
using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Exceptions;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class Book : EntityBase
    {
        protected Book() { }

        public Book(string identifier, string title, string creator)
        {
            Identifier = identifier;
            Title = title;
            Created = DateTime.Now;
            Creator = creator;
        }

        public string Identifier { get; private set; } //! Start -> Identifier
        public string Title { get; private set; } //! Start
        public string? Subtitle { get; set; } //! Start
        public string? Link { get; set; }
        public string? Auflage { get; set; }
        public DateTime Created { get; set; }

        public BookLanguage Language { get; set; }

        public string Creator { get; set; }

        public virtual ICollection<BookCategory> BookCategories { get; private set; } = new List<BookCategory>(); //! Start

        public virtual ICollection<Author> Authors { get; private set; } = new List<Author>(); //! Start

        public void SetAuthors(IEnumerable<Author> authors)
        {
            Authors = authors.ToList();
        }

        public void AddAuthor(Author author)
        {
            Authors.Add(author);
        }

        public void SetCategories(IEnumerable<Category> categories)
        {
            BookCategories = categories.Select(e => new BookCategory(this, e)).ToList();
        }

        private bool IsCategoryInList(Category category)
        {
            return BookCategories.Select(e => e.Category).Contains(category);
        }

        public void AddCategory(Category category, bool isPrimary = false)
        {
            if (IsCategoryInList(category))
                throw new BookException("Category already added to this book");

            if (isPrimary && BookCategories.Any(e => e.IsPrimary))
                throw new BookException("Book already contains a primary category, a book can only have a single primary category");

            var bc = new BookCategory(this, category, isPrimary);
            BookCategories.Add(bc);
        }

        public void RemoveCategory(Category category)
        {
            var bc = BookCategories.First(e => e.Category == category);
            if (bc is not null)
            {
                BookCategories.Remove(bc);
            }

        }

        public override string ToString()
        {
            var authors = string.Join("|", Authors.Select(e => e.Name));
            var categories = string.Join('|', BookCategories.Select(e => e.Category.Name));
            return $"Book {{ Id = {Id}, ISBN = {Identifier}, Title = {Title}, Auflage = {Auflage}, Language = {Language}, Authors = {authors}, Categories = {categories} }}";
        }
    }

    public enum BookLanguage
    {
        German,
        English
    }
}
