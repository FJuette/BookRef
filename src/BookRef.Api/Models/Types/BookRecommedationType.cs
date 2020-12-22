using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace BookRef.Api.Models.Types
{
    public class BookRecommedationType : ObjectType<BookRecommedation>
    {
        protected override void Configure(IObjectTypeDescriptor<BookRecommedation> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<BookRecommendationByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(e => e.RecommendedBook)
                .ResolveWith<BookRecommedationResolvers>(t => t.GetRecommendedBookAsync(default!, default!, default));
            descriptor.Field(e => e.SourceBook)
                .ResolveWith<BookRecommedationResolvers>(t => t.GetSourceBookAsync(default!, default!, default));
            // descriptor.Field(e => e.Note)
            //     .ResolveWith<BookRecommedationResolvers>(t => t.GetNoteAsync(default!, default!, default));

            descriptor
                 .Field(t => t.RecommendedBookId)
                 .ID(nameof(Book));
            descriptor
                 .Field(t => t.SourceBookId)
                 .ID(nameof(Book));
        }
        private class BookRecommedationResolvers
        {
            public async Task<Book?> GetRecommendedBookAsync(
                 BookRecommedation bookRecommedation,
                 BookByIdDataLoader bookById,
                 CancellationToken cancellationToken)
            {
                return await bookById.LoadAsync(bookRecommedation.RecommendedBookId, cancellationToken);
            }
            public async Task<Book?> GetSourceBookAsync(
                 BookRecommedation bookRecommedation,
                 BookByIdDataLoader bookById,
                 CancellationToken cancellationToken)
            {
                return await bookById.LoadAsync(bookRecommedation.SourceBookId, cancellationToken);
            }
            // public async Task<Note?> GetNoteAsync(
            //      BookRecommedation bookRecommedation,
            //      NoteByIdDataLoader noteById,
            //      CancellationToken cancellationToken)
            // {
            //     if (bookRecommedation.Note == null)
            //     {
            //         return null;
            //     }
            //     return await noteById.LoadAsync(bookRecommedation.Note.Id, cancellationToken);
            // }
        }
    }
}
