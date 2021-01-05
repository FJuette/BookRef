using System.Collections.Generic;

namespace BookRef.Api.Common
{
    public class Payload<T>
    {
        public Payload(IReadOnlyList<UserError>? errors = null)
        {
            Errors = errors;
        }

        public Payload(T data)
        {
            Data = data;
        }

        public IReadOnlyList<UserError>? Errors { get; }

        public T? Data { get; }
    }
}
