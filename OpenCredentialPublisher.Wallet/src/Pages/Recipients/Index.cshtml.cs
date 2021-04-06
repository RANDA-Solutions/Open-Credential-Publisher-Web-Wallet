using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class IndexModel : PageModel
    {
        private readonly EmailHelperService _emailHelperService;

        public IndexModel(EmailHelperService emailHelperService)
        {
            _emailHelperService = emailHelperService;
        }

        public string LinkId { get; set; }

        public List<RecipientModel> Recipients { get; set; }

        public async Task OnGet(string linkId)
        {
            LinkId = linkId;
            Recipients = await _emailHelperService.GetAllRecipientsAsync(User.UserId());
        }
    }
}