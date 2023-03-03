using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace helixIntegration
{
    internal class Program
    {

        static void Main(string[] args)
        {
            tikets tikets = new tikets();
            if (args.Length > 0)
            {


                Program execute = new Program();
                List<tikets> tickets = new List<tikets>();

                Login login = new Login();
                IWebDriver web = execute.initWeb();

                try
                {
                    String pathReport = @args[0];

                    tickets = execute.getTickets(web, pathReport);

                    Ticket ticketHelix = new Ticket(web);
                    GestionUsuarios servicePageTicket = new GestionUsuarios(web);
                    String catalogoUrl = "https://dceservice-dwp.onbmc.com/dwp/app/#/catalog";

                    bool isAccess = login.access(web);
                    Thread.Sleep(4000);

                    int numeroTicktes = tickets.Count;
                    string message_error= "Ticktes a procesar: " + numeroTicktes.ToString();
                    Console.WriteLine(message_error);
                    LogFile log = new LogFile(DateTime.Now, message_error, "Inicio");
                    LogFile.WriteToLog(log);

                    
                    foreach (var ticket in tickets)
                    {
                        Console.WriteLine("Procesar ticket: " + ticket.idOdt.ToString());
                        Thread.Sleep(5000);
                        log = new LogFile(DateTime.Now, "Procesar ticket: " + ticket.idOdt.ToString(), "Inicio");
                        LogFile.WriteToLog(log);

                        servicePageTicket.gestionUsuario(web, isAccess);
                        ticketHelix.crear(ticket);
                        ticketHelix.modificar(ticket);
                        ticketHelix.eliminar(ticket);
                        Thread.Sleep(5000);
                        Console.WriteLine("siguiente ticket");
                        web.Navigate().GoToUrl(catalogoUrl);
                        log = new LogFile(DateTime.Now, "Procesar ticket: " + ticket.idOdt.ToString(), "Fin");
                        LogFile.WriteToLog(log);

                    }
                    

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    LogFile log = new LogFile(DateTime.Now, e.Message.ToString(), "ERROR");
                    LogFile.WriteToLog(log);
                }
                Thread.Sleep(2000);

  
                web.Close();
                Thread.Sleep(1000);
                Process.GetCurrentProcess().Kill();
                Environment.Exit(0);
                //Fin del tiempo de proceso
            }
            else
            {
                LogFile log = new LogFile(DateTime.Now, "Debe ingresar la ruta del archivo", "ERROR");
                LogFile.WriteToLog(log);
            }
        }
        
        internal List<tikets> getTickets(IWebDriver driver, string pathFile)
        {
            Ticket ticketHelix = new Ticket(driver);
            List<tikets> tickets = new List<tikets>();
            string pathFielExcel = pathFile;
            string message_error = "Ruta del archivo fuente de tickets: " + pathFielExcel.ToString();
            Console.WriteLine(message_error);
            LogFile log = new LogFile(DateTime.Now, message_error, "Inicio");
            LogFile.WriteToLog(log);
            tickets = ticketHelix.GetSMTickets(pathFielExcel);
            return tickets;
        }

        internal IWebDriver initWeb()
        {
            ChromeOptions options = new ChromeOptions();
            //Set the argument 
            options.AddArguments("--start-maximized");
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalChromeOption("useAutomationExtension", false);
            //Set Chrome to work with headless mode
            IWebDriver web = new ChromeDriver(options);
            return web;
        }
    }
}
