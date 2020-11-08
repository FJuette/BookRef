using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public class Person : ValueObject
    {
        private Person(
            string value) =>
            Name = value;
        public string Name { get; }

        public static Result<Person> Create(
            string name) =>
            // Make validation checks and return
            name != ""
                ? Result.Failure<Person>("Must contain a value")
                : Result.Success(new Person(name));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
