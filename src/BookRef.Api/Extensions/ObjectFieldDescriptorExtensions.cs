using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookRef.Api.Extensions
{
    public static class ObjectFieldDescriptorExtensions
     {
         public static IObjectFieldDescriptor UseDbContext<TDbContext>(
             this IObjectFieldDescriptor descriptor)
             where TDbContext : DbContext
         {
             return descriptor.UseScopedService<TDbContext>(
                 create: s => s.GetRequiredService<IDbContextFactory<TDbContext>>().CreateDbContext(),
                 disposeAsync: (s, c) => c.DisposeAsync());
         }
     }
}
