using System.Collections.Generic;
using System.Threading.Tasks;
using BookRef.Api.Common;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using FluentValidation;
using HotChocolate;
using HotChocolate.Types;

namespace BookRef.Api.People
{
    public record AddPersonInput(string Name);

    [ExtendObjectType(Name = "Mutation")]
    public class PeopleMutations
    {
        [UseApplicationDbContext]
        public async Task<Payload<Person>> AddPersonAsync(
             AddPersonInput input,
             [ScopedService] BookRefDbContext context)
        {
            var person = new Person(input.Name);
            context.People.Add(person);
            await context.SaveChangesAsync();

            return new Payload<Person>(person);
        }
    }
}
