using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using HotChocolate.Data.Filters;

namespace BookRef.Api.Authors
{
    public class AuthorFilterInputType : FilterInputType<Author>
   {
      protected override void Configure(IFilterInputTypeDescriptor<Author> descriptor)
      {
            descriptor.Ignore(t => t.Id);
            descriptor.Ignore(t => t.Books);
      }
   }
}
