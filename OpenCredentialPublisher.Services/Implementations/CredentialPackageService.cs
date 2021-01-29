using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialPackageService
    {
        private readonly WalletDbContext _context;
        private readonly ILogger<CredentialPackageService> _logger;
        public CredentialPackageService(WalletDbContext context, ILogger<CredentialPackageService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UpdateCredentialPackageNameAsync(string userId, int credentialPackageId, string name)
        {
            try
            {
                var credentialPackage = await _context.CredentialPackages.FirstOrDefaultAsync(cp => cp.Id == credentialPackageId);
                if (credentialPackage == null)
                {
                    _logger.LogError("A credential package with the supplied id could not be found.", userId, credentialPackageId, name);
                }
                else if (credentialPackage.UserId != userId)
                {
                    _logger.LogError("That credential package belongs to another user.", userId, credentialPackageId, name);
                }
                else
                {
                    credentialPackage.Name = name;
                    credentialPackage.ModifiedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId, credentialPackageId, name);
            }
            return false;
        }
    }
}
