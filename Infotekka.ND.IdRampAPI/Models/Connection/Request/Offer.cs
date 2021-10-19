using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Connection.Request
{
    [Serializable]
    public class Offer
    {
        [JsonPropertyName("aliasName")]
        public string AliasName { get; set; }

        [JsonPropertyName("aliasIconUrl")]
        public string AliasIconUrl { get; set; }
    }
}
