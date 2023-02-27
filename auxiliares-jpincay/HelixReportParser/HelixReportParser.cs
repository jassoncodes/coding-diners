using HelixTicketsReportParser.Models;
using AppConfig = HelixTicketsReportParser.Config.AppConfig;
using Excel = Microsoft.Office.Interop;
using TicketsParser = HelixTicketsReportParser.Models.TicketsParser;
using HelixTicket = HelixTicketsReportParser.Models.HelixTicket;
using SMTicket = HelixTicketsReportParser.Models.SMTicket;
using Serilog;
using System.Reflection;


namespace HelixTicketsReportParser
{

    internal class HelixReportParser
    {
        static void Main(string[] args)
        {

            AppConfig appConfig = new();
            HelixTicketsFileParser helixFileParser = new();
            SMTicketsFileParser smTicketsFileParser = new();
            TicketsParser ticketsParser = new();
            ProccessHandler proccessHandler = new();

            try
            {

                appConfig.ConfigLog();

                List<HelixTicket> helixTickets = helixFileParser.GetHelixTicketsList(appConfig.inputPath, appConfig.inputFileName);

                List<SMTicket> smTickets = smTicketsFileParser.GetSMTickets(
                    appConfig.outputPath,
                    appConfig.outputFileName
                    );

                List<SMTicket> smTicketsParsed = ticketsParser.GetIdPeticion(smTickets, helixTickets);

                smTicketsFileParser.UpdateFile(
                    appConfig.outputPath,
                    appConfig.outputFileName,
                    smTicketsParsed,
                    appConfig.colIdPeticionHelix
                    );

                string pathArchivoFinal = Path.Combine(appConfig.archivoFinalPath, new string($@"{DateTime.Now:yyyy-M-d}\"));

                if(smTicketsFileParser.GenerateArchivoFinal(smTickets, pathArchivoFinal))
                {
                    Log.Information($@"Se ha generado ArchivoFinal.xls {pathArchivoFinal}\ArchivoFinal.xls");
                };

                Log.Information($"Proceso terminado con éxito!...");

                proccessHandler.KillExcelProccess();

            }
            catch (Exception e) 
            {
                Log.Error($"Main() Error: {e}");
            }
        }
    }
}