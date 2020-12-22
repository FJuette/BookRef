using BookRef.Api.Models;
using HotChocolate.Data.Filters;

namespace BookRef.Api.Books
{
    public class BookFilterInputType : FilterInputType<Book>
   {
      protected override void Configure(IFilterInputTypeDescriptor<Book> descriptor)
      {
            descriptor.Ignore(t => t.Id);
            //descriptor.Ignore(t => t.);
      }
   }
}
