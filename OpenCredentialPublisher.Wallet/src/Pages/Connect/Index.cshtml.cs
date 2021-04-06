using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.ClrWallet.Pages.Connect
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ConnectService _connectService;
        public IndexModel(ConnectService connectService, ILogger<IndexModel> logger)
        {
            _connectService = connectService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public ConnectGetModel GetModel { get; set; }
        public async Task OnGetAsync()
        {
            await Task.Delay(0);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await _connectService.ConnectAsync(this, GetModel);
                if (result.HasError)
                {
                    foreach(var error in result.ErrorMessages)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return Page();
                }
                if (result.Id.HasValue)
                    return RedirectToPage("/Credentials/Display", new  { id = result.Id.Value });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem processing this request.", GetModel);
                ModelState.AddModelError("", ex.Message);
            }
            return Page();
        }
    }
}
