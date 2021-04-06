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

namespace OpenCredentialPublisher.Services.Implementations
{
    public class LinkService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;

        public LinkService(WalletDbContext context, SchemaService schemaService)
        {
            _context = context;
            _schemaService = schemaService;
        }
        public async Task<LinkModel> AddAsync(LinkModel input)
        {
            await _context.Links.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task DeleteAsync(string id)
        {
            var item = await _context.Links
                    .Include(l => l.Shares)
                    .ThenInclude(s => s.Messages)
                    .AsNoTracking()
                    .SingleAsync(x => x.Id == id);

            _context.Links.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task<LinkModel> UpdateAsync(LinkModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<List<LinkModel>> GetAllAsync(string userId)
        {
            var result = await _context.Links
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }
        public async Task<List<LinkModel>> GetAllDeepAsync(string userId)
        {
            var result = await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Include(l => l.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ClrSet)
                .ThenInclude(c => c.CredentialPackage)
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }
        public async Task<LinkModel> GetAsync(string userId, string id)
        {
            return await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<LinkModel> GetAsync(string id)
        {
            return await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<LinkModel> GetDeepAsync(string id)
        {
            var result = await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Include(l => l.Clr)
                .ThenInclude(l => l.VerifiableCredential)
                .ThenInclude(c => c.CredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ClrSet)
                .ThenInclude(l => l.VerifiableCredential)
                .ThenInclude(c => c.CredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ClrSet)
                .ThenInclude(c => c.CredentialPackage)
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }
        public async Task<ShareModel> AddShareAsync(ShareModel input)
        {
            await _context.Shares.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }
    }
}
