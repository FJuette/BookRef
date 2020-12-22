using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using HotChocolate.Data.Filters;

namespace BookRef.Api.Books
{
    public class PersonalBookFilterInputType : FilterInputType<PersonalBook>
   {
      protected override void Configure(IFilterInputTypeDescriptor<PersonalBook> descriptor)
      {
            descriptor.Ignore(t => t.PersonalBooksId);
            //descriptor.Ignore(t => t.);
      }
   }
}
