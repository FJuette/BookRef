using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace BookRef.Api.Common
{
    public static class PayloadHelper
    {
        public static IReadOnlyList<UserError> BuildErrorList(IList<ValidationFailure> input)
        {
            return input.Select(e => new UserError(e.ErrorMessage, e.ErrorCode)).ToList();
        }

        public static IReadOnlyList<UserError> BuildSingleError(Exception ex)
        {
            return new List<UserError>{ new UserError(ex.Message, "9000") };
        }
    }

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
