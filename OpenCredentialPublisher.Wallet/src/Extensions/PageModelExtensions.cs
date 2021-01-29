using Microsoft.AspNetCore.Mvc.RazorPages;
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
            if (Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/Links/Display/{id}", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }
    }
}
