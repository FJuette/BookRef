using System.Collections.Generic;
using System.Linq;

namespace BookRef.Api.Models.ValueObjects
{
    public class Author
    {
        protected Author() { }
        public Author(string name)
        {
            Name = name;
        }
        public long Id { get; private set; }
        public string Name { get; private set; }

        public virtual ICollection<Book> Books  { get; private set; }
    }
}
