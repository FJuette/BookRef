﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BookRef.Api.Infrastructure
{
    public interface IGetClaimsProvider
    {
        long UserId { get; }
        string Username { get; }
        Guid LibraryId { get; }
    }

    public class GetClaimsFromUser : IGetClaimsProvider
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public GetClaimsFromUser(
            IHttpContextAccessor accessor)
        {
            var username = accessor.HttpContext?
                .User.Claims
                .SingleOrDefault(x => x.Type == ClaimTypes.Name)
                ?.Value;

            Username = username ?? "";

            UserId = 1;
            var libId = accessor.HttpContext?
                .User.Claims
                .SingleOrDefault(x => x.Type == "LibraryId")
                ?.Value;
            LibraryId = libId != null ? new Guid(libId) : Guid.Empty;
        }

        public long UserId { get; }
        public string Username { get; }
        public Guid LibraryId { get; }
    }
}
