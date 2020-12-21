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
    public class CategoryByIdDataLoader : BatchDataLoader<long, Category>
    {
        private readonly IDbContextFactory<BookRefDbContext> _dbContextFactory;

        public CategoryByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BookRefDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                 throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<long, Category>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {
            await using BookRefDbContext dbContext =
                 _dbContextFactory.CreateDbContext();
            return await dbContext.Categories
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
