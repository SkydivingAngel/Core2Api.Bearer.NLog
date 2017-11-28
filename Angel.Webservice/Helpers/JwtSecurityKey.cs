namespace Angel.Webservice.Helpers
{
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }
}