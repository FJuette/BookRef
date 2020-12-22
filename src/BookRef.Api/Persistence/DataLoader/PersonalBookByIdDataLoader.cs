using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Persistence.DataLoader
{
    public class PersonalBookByIdDataLoader : BatchDataLoader<long, PersonalBook>
    {
        private readonly IDbContextFactory<BookRefDbContext> _dbContextFactory;

        public PersonalBookByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BookRefDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                 throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<long, PersonalBook>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {
            await using BookRefDbContext dbContext =
                 _dbContextFactory.CreateDbContext();
            return await dbContext.PersonalBooks
                .Where(s => keys.Contains(s.PersonalBooksId))
                .ToDictionaryAsync(t => t.PersonalBooksId, cancellationToken);
        }
    }
}
