using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Categories
{
    [ExtendObjectType(Name = "Query")]
    public class CategoryQueries
    {
        [UseApplicationDbContext]
        public Task<List<Category>> GetCategoriesAsync([ScopedService] BookRefDbContext context) =>
             context.Categories.ToListAsync();

        public Task<Category> GetCategoryByIdAsync(
            [ID(nameof(Category))] long id,
            CategoryByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public Task<IReadOnlyList<Category>> GetCategoriesByIdAsync(
            [ID(nameof(Category))] long[] ids,
            CategoryByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}
