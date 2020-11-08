using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Models
{
    public class Recommedation : EntityBase
    {
        public RecommedationType Type { get; set; }

        public User Owner { get; set; }
        public Note Note { get; set; }

        public Book SourceBook { get; set; }
        public long SourceBookId { get; set; }

        public Book? RecommendedBook { get; set; }
        public long? RecommendedBookId { get; set; }

        public Person? RecommendedPerson { get; set; }
        public long? RecommendedPersonId { get; set; }

    }

    public enum RecommedationType
    {
        Book,
        Person
    }
}
