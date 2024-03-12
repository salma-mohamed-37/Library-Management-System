using System.Security.Claims;

namespace backend.Interfaces
{
    public interface ITokenService
    {
        public string GenerateNewJsonWebToken(List<Claim> claims);
    }
}
