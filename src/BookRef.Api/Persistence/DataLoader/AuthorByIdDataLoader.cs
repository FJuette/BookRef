using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Persistence.DataLoader
{
    public class AuthorByIdDataLoader : BatchDataLoader<long, Author>
    {
        private readonly IDbContextFactory<BookRefDbContext> _dbContextFactory;

        public AuthorByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BookRefDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                 throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<long, Author>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {
            await using BookRefDbContext dbContext =
                 _dbContextFactory.CreateDbContext();
            return await dbContext.Authors
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
