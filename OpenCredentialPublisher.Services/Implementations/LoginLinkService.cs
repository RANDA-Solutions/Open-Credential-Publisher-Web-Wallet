using OpenCredentialPublisher.ClrLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenCredentialPublisher.Data.Abstracts;
using System.Linq;
using OpenCredentialPublisher.Data.ViewModels.nG;
using Microsoft.AspNetCore.Http;
using IdentityModel;
using Microsoft.AspNetCore.WebUtilities;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class LoginLinkService
    {
        private readonly WalletDbContext _context;

        public LoginLinkService(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<LoginLink> CreateLoginLinkAsync(string userId, DateTime validUntil, int? credentialPackageId)
        {
            var loginLink = new LoginLink
            {
                UserId = userId,
                State = WebEncoders.Base64UrlEncode(CryptoRandom.CreateRandomKey(64)),
                Code = WebEncoders.Base64UrlEncode(CryptoRandom.CreateRandomKey(38)),
                ReturnUrl = credentialPackageId.HasValue ? $"/credentials/display/{credentialPackageId}"
                    : "/",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Claimed = false,
                IsDeleted = false,
                ValidUntil = validUntil,
            };

            await _context.LoginLinks.AddAsync(loginLink);
            await _context.SaveChangesAsync();
            _context.Entry(loginLink).State = EntityState.Detached;

            return loginLink;
        }

        public async Task<LoginLink> GetLoginLinkByStateAsync(string state)
        {
            return await _context.LoginLinks.FirstOrDefaultAsync(l => l.State == state);
        }

        public async Task<LoginLink> GetLoginLinkByCodeAsync(string code)
        {
            return await _context.LoginLinks.FirstOrDefaultAsync(l => l.Code == code);
        }

        public async Task UpdateAsync(LoginLink loginLink)
        {
            loginLink.ModifiedAt = DateTime.UtcNow;
            _context.LoginLinks.Update(loginLink);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLoginLinkAsync(LoginLink loginLink)
        {
            loginLink.Delete();
            _context.LoginLinks.Update(loginLink);
            await _context.SaveChangesAsync();
        }

        
    }
}
