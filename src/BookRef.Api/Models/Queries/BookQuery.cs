using System;
using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models.Types;
using BookRef.Api.Persistence;
// using GraphQL;
//using GraphQL.Types;

namespace BookRef.Api.Models.Queries
{
    // public class BookQuery : ObjectGraphType
    // {
    //     public BookQuery(BookRefDbContext ctx)
    //     {
    //         Field<ListGraphType<BookType>>("books",
    //             arguments: new QueryArguments(new List<QueryArgument>
    //             {
    //                 new QueryArgument<LanguageType>{Name = "language"},
    //                 new QueryArgument<StringGraphType>{Name = "isbn"},
    //             }),
    //             resolve: context =>
    //             {
    //                 var result = ctx.Books.AsQueryable();
    //                 var lang = context.GetArgument<Language?>("language");
    //                 if (lang.HasValue)
    //                 {
    //                     result = result.Where(e => e.Language == lang.Value);
    //                 }

    //                 var isbn = context.GetArgument<string>("isbn");
    //                 if (isbn == "aaa")
    //                 {
    //                     result = result.Where(e => e.Isbn == isbn);
    //                 }
    //                 return result;
    //             });
    //     }
    // }
}
