using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class PersonalBookType : ObjectType<PersonalBook>
    {
        protected override void Configure(IObjectTypeDescriptor<PersonalBook> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<PersonalBookByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(e => e.Book)
                .ResolveWith<PersonalBookResolvers>(t => t.GetRecommendedBookAsync(default!, default!, default));

            descriptor
                 .Field(t => t.BookId)
                 .ID(nameof(Book));
        }
        private class PersonalBookResolvers
        {
            public async Task<Book?> GetRecommendedBookAsync(
                 PersonalBook personalBook,
                 BookByIdDataLoader bookById,
                 CancellationToken cancellationToken)
            {
                return await bookById.LoadAsync(personalBook.BookId, cancellationToken);
            }
        }
    }
}
