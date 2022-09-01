using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, message, args);
            if (ex.InnerException != null)
                logger.LogException(ex.InnerException, message, args);
        }

        public static void LogException<T>(this ILogger<T> logger, Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, message, args);
            if (ex.InnerException != null)
                logger.LogException(ex.InnerException, message, args);
        }
    }
}
