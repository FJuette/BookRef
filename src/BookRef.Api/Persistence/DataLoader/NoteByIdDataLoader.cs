using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Persistence.DataLoader
{
    public class NoteByIdDataLoader : BatchDataLoader<long, Note>
    {
        private readonly IDbContextFactory<BookRefDbContext> _dbContextFactory;

        public NoteByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BookRefDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                 throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<long, Note>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {
            await using BookRefDbContext dbContext =
                 _dbContextFactory.CreateDbContext();
            return await dbContext.Notes
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
