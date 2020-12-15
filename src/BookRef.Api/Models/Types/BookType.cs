using GraphQL.Types;

namespace BookRef.Api.Models.Types
{
    public class LanguageType: EnumerationGraphType<Language>
    {
        public LanguageType()
        {
            Description = "Language from the book";
        }
    }
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(x => x.Id);
            Field(x => x.Isbn);
            Field(x => x.Title);
            Field(x => x.Link);
            Field(x => x.Auflage);
            Field(x => x.Created);
            Field<LanguageType>(nameof(Book.Language));
        }


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
