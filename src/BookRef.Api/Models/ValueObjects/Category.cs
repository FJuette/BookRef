using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public class Category : ValueObject
    {
        private Category(
            string value) =>
            Name = value;
        public string Name { get; }

        public static Result<Category> Create(
            string name) =>
            // Make validation checks and return
            name != ""
                ? Result.Failure<Category>("Must contain a value")
                : Result.Success(new Category(name));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
