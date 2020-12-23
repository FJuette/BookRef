using System;

namespace BookRef.Api.Models
{
    public abstract class BaseRecommedation : EntityBase
    {
        public RecommedationType Type { get; protected set; }

        public Guid PersonalLibraryId { get; set; }
        public virtual Note? Note { get; set; }
        public long NoteId { get; set; }
        public virtual Book SourceBook { get; set; }
        public long SourceBookId { get; set; }
    }

    public enum RecommedationType
    {
        Book,
        Person
    }
}
