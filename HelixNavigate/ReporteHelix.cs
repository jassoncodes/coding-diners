
using helixIntegration;
using OpenQA.Selenium;
using Serilog;
namespace HelixNavigate
{
     class ReporteHelix:HelperRpa
    {
        private IWebDriver driverInterface;
        public ReporteHelix(IWebDriver driver) : base(driver) => driverInterface = driver;
        public void ConfigLog()
        {
            string path = @"E:\RECURSOS ROBOT\LOGS\MESA_SERVICIO\GESTIONDEUSUARIOS\\";
            string fecha_log = $@"{DateTime.Now:yyyy-M-d}\";
            string logPathFinal = Path.Combine(path, fecha_log);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"{logPathFinal}{System.AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:yyyyMMdd-HHmm}.log",
                                 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Log configurado...");
        }
        internal bool accessReport(IWebDriver web)
        {
            Program execute = new Program();
            execute.ConfigLog();
            string reportInit = "https://dceservice-smartit.onbmc.com/smartit/app/#/";
            //string reportPageDowload = "https://dceservice-sr.onbmc.com/RunDashboard.i4;cb259454-e228-4b80-9f4b-85fdbcc38e4a=9e7d6644-bd62-444f-aaed-17e339e093e1?dashUUID=b47f4f74-87df-4777-8f34-88f3c7ca247e&primaryOrg=1&clientOrg=13001&arhost=onbmc-s&port=46262&midtier=dceservice-qa.onbmc.com&protocol=https";
            string reportPageDowload = @"https://dceservice-sr.onbmc.com/RunDashboard.i4%d3cb259454-e228-4b80-9f4b-85fdbcc38e4a=9e7d6644-bd62-444f-aaed-17e339e093e1?dashUUID=b47f4f74-87df-4777-8f34-88f3c7ca247e&primaryOrg=1&clientOrg=13001&arhost=onbmc-s&port=46262&midtier=dceservice-qa.onbmc.com&protocol=https";
            string message = "";
            message = "Ingresando a la pagina de reporte";
            Log.Information($"{message}");
            web.Navigate().GoToUrl(reportInit);
            Thread.Sleep(3000);
            web.Navigate().GoToUrl(reportPageDowload);
            Thread.Sleep(3000);

            findFieldClick("//li[@id='fave']/div[1]");
            Thread.Sleep(3000);
            findFieldClick("//div[@id='myFavouritesScrollableArea']/div[1]/div[1]/ul[1]/li[1]/p[1]");
            Thread.Sleep(3000);
            findFieldClick("//div[@id='pagecontent']/div[2]/div[3]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/img[1]\r\n");


            return false;
        }
    }
}
