using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Security.Jwt
{
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string SecretKey { get; init; } = string.Empty;

        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public int ExpirationInMinutes { get; init; }
    }
}
