using BookRef.Api.Models.ValueObjects;
using HotChocolate.Data.Filters;

namespace BookRef.Api.People
{
    public class PersonFilterInputType : FilterInputType<Person>
   {
      protected override void Configure(IFilterInputTypeDescriptor<Person> descriptor)
      {
            descriptor.Ignore(t => t.Id);
      }
   }
}
