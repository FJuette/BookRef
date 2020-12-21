using System;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class PersonRecommedation : BaseRecommedation
    {
        public PersonRecommedation()
        {
            this.Type = RecommedationType.Person;
        }
        public virtual Person? RecommendedPerson { get; set; }
        public long? RecommendedPersonId { get; set; }

    }
}
