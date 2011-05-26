using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace NetPonto.Web.Infrastructure.Logging
{
    public interface ILogger
    {
        void NLogLogger();

        void Info(string message);

        void Warning(string message);

        void Debug(string message);

        void Error(string message);

        void Fatal(string message);

        void Error(Exception  ex);
    }
}
