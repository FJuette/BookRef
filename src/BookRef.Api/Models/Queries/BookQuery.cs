using BookRef.Api.Models.Types;
using BookRef.Api.Persistence;
using GraphQL.Types;

namespace BookRef.Api.Models.Queries
{
    public class BookQuery : ObjectGraphType
    {
        public BookQuery(BookRefDbContext ctx)
        {
            Field<ListGraphType<BookType>>("books", resolve: context => ctx.Books);
        }
    }
}
