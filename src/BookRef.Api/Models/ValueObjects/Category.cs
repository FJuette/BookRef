using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public record Category
    {
        protected Category()
        {

        }
        public Category(string name)
        {
            Name = name;
        }
        public long Id { get; init; }
        public string Name { get; init; }
    }
}
