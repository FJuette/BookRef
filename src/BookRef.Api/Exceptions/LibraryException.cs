using System;

namespace BookRef.Api.Exceptions
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message)
        {

        }
    }
}
