using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class RecipientService
    {
        private readonly WalletDbContext _context;
        public RecipientService(WalletDbContext context)
        {
            _context = context;
        }
        public async Task<(RecipientModel recipient, bool alreadyExists)> AddAsync(RecipientModel input)
        {
            bool newRecipient = false;
            if ((newRecipient = !await _context.Recipients.AnyAsync(x => x.UserId == input.User.Id && x.Email == input.Email)))
            {
                await _context.Recipients.AddAsync(input);
                await _context.SaveChangesAsync();
            }
            return (input, !newRecipient);
        }
        public async Task DeleteAsync(int id)
        {
            var item = await _context.Recipients
                    .SingleAsync(x => x.Id == id);

            item.Delete();
           // _context.Recipients.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task<RecipientModel> UpdateAsync(RecipientModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<List<RecipientModel>> GetAllAsync(string userId)
        {
            var result = await _context.Recipients
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<RecipientModel> GetAsync(int id)
        {
            return await _context.Recipients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
