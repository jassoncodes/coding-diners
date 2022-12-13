using RiesgoPichinchaQuoteParser.Models;
using System.Security.Cryptography.X509Certificates;
using Log = Serilog.LoggerConfiguration;

namespace RiesgoPichinchaQuoteParser
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            
            FileProccessor fileProccessor = new FileProccessor();
            LoggerConfigurator logger = new LoggerConfigurator();

            logger.configureLog();

            fileProccessor.ReadFiles(fileProccessor.inputPath);
            
        }
    }
}