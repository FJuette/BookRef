using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public sealed class Author : ValueObject
    {
        private Author(
            string value) =>
            Name = value;
        public string Name { get; }

        public static Result<Author> Create(
            string name) =>
            // Make validation checks and return
            name != ""
                ? Result.Failure<Author>("Must contain a value")
                : Result.Success(new Author(name));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
