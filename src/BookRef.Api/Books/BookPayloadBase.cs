using System.Collections.Generic;
using BookRef.Api.Common;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Authors
{
    public class BookPayloadBase : Payload
    {
        protected BookPayloadBase(Book book)
        {
            Book = book;
        }

        protected BookPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Book? Book { get; }
    }
}
