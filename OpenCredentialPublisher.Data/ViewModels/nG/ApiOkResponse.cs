using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result, string message = null, string redirectUrl = null)
            : base(200, message, redirectUrl)
        {
            Result = result;
             
        }
    }
}
