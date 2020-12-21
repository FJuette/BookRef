using System.Reflection;
using BookRef.Api.Persistence;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace BookRef.Api.Extensions
{
    public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<BookRefDbContext>();
        }
    }
}
