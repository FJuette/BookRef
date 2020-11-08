using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace BookRef.Api.Models.ValueObjects
{
    public class Speaker : ValueObject
    {
        private Speaker(
            string value) =>
            Name = value;
        public string Name { get; }

        public static Result<Speaker> Create(
            string name) =>
            // Make validation checks and return
            name != ""
                ? Result.Failure<Speaker>("Must contain a value")
                : Result.Success(new Speaker(name));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
