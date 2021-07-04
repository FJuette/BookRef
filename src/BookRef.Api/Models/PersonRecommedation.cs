using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class PersonRecommedation : BaseRecommedation
    {
        protected PersonRecommedation() { }

        public PersonRecommedation(Author person)
        {
            this.Type = RecommedationType.Person;
            RecommendedPerson = person;
        }
        public virtual Author RecommendedPerson { get; private set; }
        public long RecommendedPersonId { get; private set; }

    }
}
