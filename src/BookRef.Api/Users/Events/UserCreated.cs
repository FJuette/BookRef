using System;

namespace BookRef.Api.Users.Events
{
    public record UserCreated
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
    }
}
