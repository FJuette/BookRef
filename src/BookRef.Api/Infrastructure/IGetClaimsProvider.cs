using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BookRef.Api.Infrastructure
{
    public interface IGetClaimsProvider
    {
        long UserId { get; }
        Guid LibraryId { get; }
    }

    public class GetClaimsFromUser : IGetClaimsProvider
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public GetClaimsFromUser(
            IHttpContextAccessor accessor)
        {
            // var username = accessor.HttpContext?
            //     .User.Claims
            //     .SingleOrDefault(x => x.Type == ClaimTypes.Name)
            //     ?.Value;

            // UserId = string.IsNullOrEmpty(username) ? "Admin" : username;
            UserId = 1;
            LibraryId = new Guid("EE471115-0425-489B-931A-8B3F7F187205");
        }

        public long UserId { get; }
        public Guid LibraryId { get; }
    }
}
