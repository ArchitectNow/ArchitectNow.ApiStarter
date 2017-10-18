using Microsoft.IdentityModel.Tokens;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public class JwtSigningKey: SymmetricSecurityKey
    {
        public JwtSigningKey(byte[] key) : base(key)
        {
        }
    }
}