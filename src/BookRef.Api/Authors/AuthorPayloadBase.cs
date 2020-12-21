using System.Collections.Generic;
using BookRef.Api.Common;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Authors
{
    public class AuthorPayloadBase : Payload
    {
        protected AuthorPayloadBase(Author author)
        {
            Author = author;
        }

        protected AuthorPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Author? Author { get; }
    }
}
