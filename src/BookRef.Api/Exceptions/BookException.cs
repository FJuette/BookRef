using System;

namespace BookRef.Api.Exceptions
{
    public class BookException : Exception
    {
        public BookException(string message) : base(message)
        {

        }
    }
}
