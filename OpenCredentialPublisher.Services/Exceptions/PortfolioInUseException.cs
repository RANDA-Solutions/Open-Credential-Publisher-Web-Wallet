using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class PortfolioInUseException: Exception
    {
        public String Address { get; set; }
        public PortfolioInUseException(String address) : base($"Portfolio {address} is already assigned with another user.")
        {

        }
    }
}
