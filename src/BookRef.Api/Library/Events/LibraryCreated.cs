using System;

namespace BookRef.Api.Library.Events
{
    public record LibraryCreated(Guid Id, long UserId);
}
