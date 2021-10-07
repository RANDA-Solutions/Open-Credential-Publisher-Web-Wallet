using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet
{
    public class CustomModelError
    {
        private OkObjectResult CustomErrorResponse(ActionContext actionContext)
        {  
            return  new OkObjectResult(new ApiBadRequestResponse(actionContext.ModelState));
        }
    }
}
