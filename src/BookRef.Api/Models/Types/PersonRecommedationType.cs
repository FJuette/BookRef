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
            descriptor.Field(e => e.Note)
                .ResolveWith<PersonRecommedationResolvers>(t => t.GetNoteAsync(default!, default!, default));

            descriptor
                 .Field(t => t.RecommendedPersonId)
                 .ID(nameof(Author));
            descriptor
                 .Field(t => t.SourceBookId)
                 .ID(nameof(Book));
            descriptor
                 .Field(t => t.NoteId)
                 .ID(nameof(Note));
        }
        private class PersonRecommedationResolvers
        {
            public async Task<Author?> GetRecommendedPersonAsync(
                 PersonRecommedation bookRecommedation,
                 AuthorByIdDataLoader personById,
                 CancellationToken cancellationToken)
            {
                return await personById.LoadAsync(bookRecommedation.RecommendedPersonId, cancellationToken);
            }
            public async Task<Book?> GetSourceBookAsync(
                 PersonRecommedation personRecommedation,
                 BookByIdDataLoader bookById,
                 CancellationToken cancellationToken)
            {
                return await bookById.LoadAsync(personRecommedation.SourceBookId, cancellationToken);
            }

            public async Task<Note?> GetNoteAsync(
                 PersonRecommedation personRecommedation,
                 NoteByIdDataLoader noteById,
                 CancellationToken cancellationToken)
            {
                return await noteById.LoadAsync(personRecommedation.NoteId, cancellationToken);
            }
        }
    }
}
