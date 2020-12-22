using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookRef.Api.Extensions;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Persistence.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BookRef.Api.Models.Types
{
    public class NoteType : ObjectType<Note>
    {
        protected override void Configure(IObjectTypeDescriptor<Note> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<NoteByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
        }
    }
}
