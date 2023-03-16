using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlDateGiftCard.Models
{
    internal class AppLogger
    {
        public static void ConfigLog(string logPath)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"{logPath}{System.AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:yyyyMMdd-HHmm}.log",
                                 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Log configurado...");
        }
    }
}
