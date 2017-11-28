namespace Angel.Webservice.Helpers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;

    public sealed class JwtToken
    {
        private readonly JwtSecurityToken token;

        internal JwtToken(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime ValidTo => token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(token);
    }
}
