using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Extensions
{
    public static class PageModelExtensions
    {
        public static string GetLinkUrl(this PageModel model, string id)
        {
            var Request = model.Request;
            return LinkService.GetLinkUrl(Request, id);
        }
    }
}
