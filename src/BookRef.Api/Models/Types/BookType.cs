using GraphQL.Types;

namespace BookRef.Api.Models.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(x => x.Id);
            Field(x => x.ISBN);
            Field(x => x.Title);
            Field(x => x.Link);
            Field(x => x.Auflage);
            Field(x => x.Created);
            Field(x => x.Language);
            //Field<LanguageType>(nameof(Book.Language));
        }

        // public class LanguageType: EnumerationGraphType<Language>
        // {

        // }

        /*
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        //public byte[] Image { get; set; }
        public string Auflage { get; set; }
        public DateTime Created { get; set; }

        public Language Language { get; set; }

        public User Creator { get; set; }
        */
    }

}
