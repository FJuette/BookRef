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
    public class AuthorsByIdDataLoader : BatchDataLoader<long, Author>
    {
        private readonly BookRefDbContext _context;

        public AuthorsByIdDataLoader(
            IBatchScheduler batchScheduler,
            BookRefDbContext context)
            : base(batchScheduler)
        {
            _context = context;
        }

        protected override async Task<IReadOnlyDictionary<long, Author>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {

            return await _context.Authors
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
