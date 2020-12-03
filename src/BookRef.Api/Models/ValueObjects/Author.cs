using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public record Author
    {
        protected Author()
        {

        }
        public Author(string name)
        {
            Name = name;
        }
        public long Id { get; init; }
        public string Name { get; init; }
    }
}
