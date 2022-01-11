using Microsoft.Extensions.Logging;
using System;

namespace Auction.WepApi.Logs
{
    /// <summary>
    /// Log info class
    /// </summary>
    public static class LogInfo
    {
        /// <summary>
        /// Method for log info into file
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logger"></param>
        public static void LogInfoMethod(Exception exception, ILogger logger)
        {
            logger.LogInformation("Message: " + exception.Message);
            logger.LogInformation("In method: " + exception.TargetSite);
        }
    }
}
