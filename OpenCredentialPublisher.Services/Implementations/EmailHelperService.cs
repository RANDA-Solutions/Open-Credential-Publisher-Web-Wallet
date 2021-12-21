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
    public class EmailHelperService
    {
        private readonly WalletDbContext _context;

        public EmailHelperService(WalletDbContext context)
        {
            _context = context;
        }
        public async Task<List<RecipientModel>> GetAllRecipientsAsync(string userId)
        {
            return await _context.Recipients.Where(r => r.UserId == userId).ToListAsync();
        }
        
        public async Task<RecipientModel> GetRecipientAsync(string userId, int id)
        {
            return await _context.Recipients.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id);
        }
        
        public async Task<RecipientModel> AddRecipientAsync(RecipientModel input)
        {
            await _context.Recipients.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task DeleteRecipientAsync(RecipientModel input)
        {
            var shares = _context.Shares.Include(s => s.Messages).Where(s => s.RecipientId == input.Id).ToList();
            shares.ForEach(s => s.Delete());
            //_context.RemoveRange(shares);
            input.Delete();
            //_context.Recipients.Remove(input);
            await _context.SaveChangesAsync();
        }
        public async Task<RecipientModel> UpdateRecipientAsync(RecipientModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<MessageModel> AddMessageAsync(MessageModel input)
        {
            await _context.Messages.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<MessageModel> UpdateMessageAsync(MessageModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
    }
}
