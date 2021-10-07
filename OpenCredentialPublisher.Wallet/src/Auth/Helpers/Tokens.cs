using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCredentialPublisher.Wallet.Models.Account;

namespace OpenCredentialPublisher.Wallet.Auth.Helpers
{
    public class Tokens
    {
      public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory,string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
      {
        var response = new
        {
          id = identity.Claims.Single(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Id).Value,
          profile = new {
            name = identity.Claims.Single(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Name).Value,
            preferredName = identity.Claims.Single(c => c.Type == Constants.Strings.JwtClaimIdentifiers.PreferredName).Value
          },
          auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
          expires_in = (int)jwtOptions.ValidFor.TotalSeconds
        };

        return JsonConvert.SerializeObject(response, serializerSettings);
      }
    }
}
