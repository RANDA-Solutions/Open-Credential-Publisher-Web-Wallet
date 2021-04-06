using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityModel;
using OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.Models
{
    /// <summary>
    /// Represents a Shareable link to a CLR.
    /// </summary>
    public class LinkViewModel
    {
        public LinkModel Link { get; set; }
        /// <summary>
        /// The CLR this link points to.
        /// </summary>
        public virtual ClrViewModel ClrVM { get; set; }

        public static LinkViewModel FromLinkModel(LinkModel link)
        {
            return new LinkViewModel()
            {
                Link = link,
                ClrVM = ClrViewModel.FromClrModel(link.Clr)
            };
        }
    }
}
