using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public record Person
    {
        protected Person()
        {

        }
        public Person(string name)
        {
            Name = name;
        }
        public long Id { get; init; }
        public string Name { get; init; }
    }
    // Whishlist?
}
