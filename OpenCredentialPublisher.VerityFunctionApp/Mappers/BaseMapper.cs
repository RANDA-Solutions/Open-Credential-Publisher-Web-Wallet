using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialPublisher.VerityFunctionApp.Mappers
{
    public abstract class BaseMapper
    {

        protected string AddressToString(AddressDType address)
        {
            if (address == null)
                return null;

            var stringBuilder = new StringBuilder()
                .AppendLine(address.StreetAddress)
                .AppendLine($"{address.AddressLocality}, {address.AddressRegion} {address.PostalCode}")
                .AppendLine(address.AddressCountry ?? String.Empty);
            return stringBuilder.ToString();
        }

        protected (string parentOrg, string official, string parentIdentifiers, string studentIdentifiers) GetAdditionalProperties(ClrDType clr)
        {
            string parentOrg = null,
                       official = null,
                       parentIdentifiers = null;

            if (clr?.Publisher?.AdditionalProperties != null)
            {
                var additionalProperties = clr?.Publisher?.AdditionalProperties;
                if (additionalProperties.TryGetValue("parentOrg", out object parentOrgValue))
                {
                    parentOrg = parentOrgValue.ToString();
                }

                if (additionalProperties.TryGetValue("official", out object officialValue))
                {
                    official = officialValue.ToString();
                }

                if (additionalProperties.Any(ap => ap.Key.Contains("identifiers", StringComparison.OrdinalIgnoreCase)))
                {
                    var identifiers = clr.Publisher.AdditionalProperties.FirstOrDefault(ap => ap.Key.Contains("identifiers", StringComparison.OrdinalIgnoreCase));
                    parentIdentifiers = identifiers.Value.ToString();
                }
            }

            string studentIdentifiers = null;
            if (clr?.Learner?.AdditionalProperties != null && clr.Learner.AdditionalProperties.Any(ap => ap.Key.Contains("identifiers", StringComparison.OrdinalIgnoreCase)))
            {
                var identifiers = clr.Learner.AdditionalProperties.FirstOrDefault(ap => ap.Key.Contains("identifiers", StringComparison.OrdinalIgnoreCase));
                studentIdentifiers = identifiers.Value.ToString();
            }
            return (parentOrg, official, parentIdentifiers, studentIdentifiers);
        }
    }
}
