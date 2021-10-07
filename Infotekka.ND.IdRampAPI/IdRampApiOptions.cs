using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotekka.ND.IdRampAPI
{
    public class IdRampApiOptions
    {
        public const string Section = "IdRampApi";
        public string ApiBaseUri { get; set; }
        public string BearerToken { get; set; }
        public string IconUrl { get; set; }
    }
}
