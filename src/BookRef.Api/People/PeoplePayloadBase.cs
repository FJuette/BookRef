using System.Collections.Generic;
using BookRef.Api.Common;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.People
{
    public class PeoplePayloadBase : Payload
    {
        protected PeoplePayloadBase(Person person)
        {
            Person = person;
        }

        protected PeoplePayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Person? Person { get; }
    }
}
