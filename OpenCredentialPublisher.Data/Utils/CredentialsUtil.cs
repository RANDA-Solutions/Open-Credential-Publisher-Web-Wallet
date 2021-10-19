using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Utils
{
    public static class CredentialsUtil
    {
        public static ClrDType GetRawClr(ClrModel clr)
        {
            var rawClr = new ClrDType();

            if (clr == null)
            {
                return rawClr;
            }

            if (!string.IsNullOrEmpty(clr.SignedClr))
            {
                rawClr = clr.SignedClr.DeserializePayload<ClrDType>();
            }
            else
            {
                rawClr = JsonSerializer.Deserialize<ClrDType>(clr.Json);
            }
            return rawClr;
        }
    }
}
