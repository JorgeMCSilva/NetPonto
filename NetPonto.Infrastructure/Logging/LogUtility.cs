using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using NLog;

namespace NetPonto.Web.Infrastructure.Logging
{
    public class LogUtility : ILogger
    {
        private Logger _logger;

        void ILogger.NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        void ILogger.Info(string message)
        {
            _logger.Info(message);
        }

        void ILogger.Warning(string message)
        {
            _logger.Warn(message);
        }

        void ILogger.Debug(string message)
        {
            _logger.Debug(message);
        }

        void ILogger.Error(string message)
        {
            _logger.Error(message);
        }

        void ILogger.Fatal(string message)
        {
            _logger.Fatal(message);
        }

        void ILogger.Error(Exception ex)
        {
            _logger.Error(LogUtility.BuildExceptionMessage(ex));
        }

        public static string BuildExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();

            sb.Append("Error:\n");
            sb.Append(ex.Message);
            sb.Append("\nStackTrace:\n");
            sb.Append(ex.StackTrace);

            return sb.ToString();
        }
    }
}