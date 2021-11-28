﻿namespace Core.Utilities.Security.Jwt
{
    public class TokenOptions
    {
        public TokenOptions()
        {
        }

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; }

    }
}
