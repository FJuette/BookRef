using System.Collections.Generic;
using BookRef.Api.Models.Relations;

namespace BookRef.Api.Models.ValueObjects
{
    public class Category
    {
        protected Category() { }
        public Category(string name)
        {
            Name = name;
        }
        public long Id { get; private set; }
        public string Name { get; private set; }
        public virtual ICollection<BookCategory> BookCategories { get; private set; } = new List<BookCategory>();
    }
}
