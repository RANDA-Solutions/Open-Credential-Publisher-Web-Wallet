using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class ConnectGetModel
    {
        [FromQuery]
        public string Endpoint { get; set; }
        [FromQuery]
        public string Scope { get; set; }
        [FromQuery]
        public string Payload { get; set; }
        [FromQuery]
        public string Issuer { get; set; }
        [FromQuery]
        public string Method { get; set; }
    }
}
