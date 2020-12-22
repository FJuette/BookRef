using System.Collections.Generic;
using BookRef.Api.Common;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Categories
{
    public class CategoryPayloadBase : Payload
    {
        protected CategoryPayloadBase(Category category)
        {
            Category = category;
        }

        protected CategoryPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Category? Category { get; }
    }
}
