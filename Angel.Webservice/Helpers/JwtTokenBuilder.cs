namespace Angel.Webservice.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;

    public sealed class JwtTokenBuilder
    {
        private SecurityKey securityKey;
        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private readonly Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 5;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            Enumerable.Union(this.claims, claims);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }

        public JwtToken Build()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(Enumerable.Select(this.claims, item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                              issuer: issuer,
                              audience: audience,
                              claims: claims,
                              notBefore: DateTime.Now,
                              expires: DateTime.Now.Add(TimeSpan.FromMinutes(expiryInMinutes)),
                              signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }
        private void EnsureArguments()
        {
            if (securityKey == null)
                throw new ArgumentNullException(@"Security Key");

            if (string.IsNullOrEmpty(this.subject))
                throw new ArgumentNullException(@"Subject");

            if (string.IsNullOrEmpty(this.issuer))
                throw new ArgumentNullException(@"Issuer");

            if (string.IsNullOrEmpty(this.audience))
                throw new ArgumentNullException(@"Audience");
        }
    }
}
