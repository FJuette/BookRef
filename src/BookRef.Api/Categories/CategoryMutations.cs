using System.Collections.Generic;
using System.Threading.Tasks;
using BookRef.Api.Common;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using FluentValidation;
using HotChocolate;
using HotChocolate.Types;

namespace BookRef.Api.Categories
{
    public record AddCategoryInput(string Name);

    [ExtendObjectType(Name = "Mutation")]
    public class CategoryMutations
    {
        [UseApplicationDbContext]
        public async Task<AddCategoryPayload> AddCategoryAsync(
             AddCategoryInput input,
             [ScopedService] BookRefDbContext context)
        {
            var category = new Category(input.Name);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return new AddCategoryPayload(category);
        }
    }

    public class AddCategoryPayload : Payload
    {
        public AddCategoryPayload(Category category)
        {
            Category = category;
        }
        public AddCategoryPayload(IReadOnlyList<UserError> errors)
             : base(errors)
         {
         }

        public Category? Category { get; }
    }
}
