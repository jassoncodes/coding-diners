using HelixFileParser = ActualizarReqSM.Models.HelixFileParser;
using WebSM = ActualizarReqSM.NavigatorSM.WebSM;
using RPAHelper = ActualizarReqSM.NavigatorSM.HelperRpa;
using DriverWeb = ActualizarReqSM.NavigatorSM.DriverWeb;
using ActualizarReqSM.Models;
using Serilog;
using AppParams = ActualizarReqSM.Models.AppParams;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ActualizarReqSM.NavigatorSM;

namespace ActualizarReqSM
{
    internal class ActualizarReqSM
    {

        static void Main(string[] args)
        {

            AppParams appParams = new();
            
            WebSM webSM = new();

            try
            {
                if (args.Length > 0 )
                {
                    appParams.rutaArchivoBase = args[0];
                    appParams.rutaLog = Path.Combine(args[1], new string($@"{DateTime.Now:yyyy-M-d}\"));
                    webSM.urlSM = args[2];
                    webSM.userSM = args[3];
                    webSM.passSM = args[4];
                    AppParams.ConfigLog(appParams.rutaLog);
                }


                Log.Information($"Obteniendo lista de tickets de archivo base {appParams.rutaArchivoBase}");
                    
                List<HelixTicket> helixTicketList = new HelixFileParser().GetTicketsList(appParams.rutaArchivoBase);


                bool loggedIn = webSM.LogInSM(webSM.urlSM, webSM.userSM, webSM.passSM);


                DateTime timeIni = DateTime.Now;

                if (loggedIn)
                {
                    webSM.AbrirPanelBusquedaPeticion();

                    Log.Information($"Logged In Service Manager ");
                    
                    foreach (HelixTicket ticket in helixTicketList)
                    {
                        Log.Information($"Ticket leido: IDODT {ticket.idOdt} REQ: {ticket.noReq}");
                        webSM.BuscarPeticion(ticket);
                        webSM.ActualizarPeticion(ticket);

                    }

                }

                DateTime timeFin = DateTime.Now;

                string tiempoTranscurrido = (timeFin - timeIni).ToString();


                webSM.LogOutSM();

                Log.Information($"Proceso terminado...\nTiempo de ejecucion: {tiempoTranscurrido}");

            }
            catch (Exception e)
            {
                Log.Error($"ActualizarReqSM Error: No se pudo gestionar actualizacion\n{e}");
            }

        }
    }
}