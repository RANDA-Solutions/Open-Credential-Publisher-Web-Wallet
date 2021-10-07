using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    public class ApiModelValidationException : Exception
    {
        public ModelStateDictionary ModelState { get; private set; }

        public ApiModelValidationException(ModelStateDictionary modelState) : base($"There were {modelState.Select(ms => ms.Value.Errors.Count).Sum()} validation error(s).")
        {
            ModelState = modelState;
        }

        public ApiModelValidationException(ModelStateDictionary modelState, string message)
            : base(message)
        {
            ModelState = modelState;
        }

        public ApiModelValidationException(ModelStateDictionary modelState, string message, Exception inner)
            : base(message, inner)
        {
            ModelState = modelState;
        }

    }

    public class ApiModelNotFoundException : Exception
    {
        public ApiModelNotFoundException(string message)
            : base(message)
        {
        }

        public ApiModelNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }

    public class ApiModelUnauthorizedAccessException : Exception
    {
        public ApiModelUnauthorizedAccessException(string message)
            : base(message)
        {
        }

        public ApiModelUnauthorizedAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
