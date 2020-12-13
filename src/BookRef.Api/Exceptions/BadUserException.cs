using System;

namespace BookRef.Api.Exceptions
{
    public class BadUserException : Exception
    {
        public BadUserException()
        {
        }

        public BadUserException(
            string user)
            : base($"User '{user}' not found.")
        {
        }
    }
}
