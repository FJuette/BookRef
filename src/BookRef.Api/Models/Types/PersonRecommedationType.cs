using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class PersonRecommedationType : ObjectType<PersonRecommedation>
    {
        protected override void Configure(IObjectTypeDescriptor<PersonRecommedation> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<PersonRecommendationByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(e => e.RecommendedPerson)
                .ResolveWith<PersonRecommedationResolvers>(t => t.GetRecommendedPersonAsync(default!, default!, default));
            descriptor.Field(e => e.SourceBook)
                .ResolveWith<PersonRecommedationResolvers>(t => t.GetSourceBookAsync(default!, default!, default));

            descriptor
                 .Field(t => t.RecommendedPersonId)
                 .ID(nameof(Book));
            descriptor
                 .Field(t => t.SourceBookId)
                 .ID(nameof(Book));
        }
        private class PersonRecommedationResolvers
        {
            public async Task<Person?> GetRecommendedPersonAsync(
                 PersonRecommedation bookRecommedation,
                 PersonByIdDataLoader personById,
                 CancellationToken cancellationToken)
            {
                return await personById.LoadAsync(bookRecommedation.RecommendedPersonId, cancellationToken);
            }
            public async Task<Book?> GetSourceBookAsync(
                 BookRecommedation bookRecommedation,
                 BookByIdDataLoader bookById,
                 CancellationToken cancellationToken)
            {
                return await bookById.LoadAsync(bookRecommedation.SourceBookId, cancellationToken);
            }
        }
    }
}
