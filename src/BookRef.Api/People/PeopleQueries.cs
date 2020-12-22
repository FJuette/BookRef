using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.People
{
    [ExtendObjectType(Name = "Query")]
    public class PersonQueries
    {
        [UseApplicationDbContext]
        public Task<List<Person>> GetPeople([ScopedService] BookRefDbContext context) =>
             context.People.ToListAsync();

        public Task<Person> GetPersonAsync(
            [ID(nameof(Person))] long id,
            PersonByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}

