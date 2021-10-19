
using OpenCredentialPublisher.Data.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(ApplicationUser user);
    }
}
