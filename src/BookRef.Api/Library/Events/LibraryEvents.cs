using System;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;

namespace BookRef.Api.Library.Events
{
    public record LibraryCreated(Guid LibraryId, long UserId);
    public record BookAdded(Guid LibraryId, Book Book, BookStatus Status, string? ColorCode);
}
