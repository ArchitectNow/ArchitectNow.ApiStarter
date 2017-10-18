using Microsoft.IdentityModel.Tokens;

namespace ArchitectNow.ApiStarter.Common.Models.Security
{
    public class JwtSigningKey: SymmetricSecurityKey
    {
        public JwtSigningKey(byte[] key) : base(key)
        {
        }
    }
}