﻿using System.Security.Claims;

namespace SharedKernel.Authentication.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdString, out var userId) ? userId
                : Guid.Empty;
        }
    }
}
