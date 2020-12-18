using System;
using BookRef.Api.Models;

namespace BookRef.Api.Library.Events
{
    public record LibraryCreated(Guid LibraryId, long UserId);
    public record BookAdded(Guid LibraryId, long BookId);
}
