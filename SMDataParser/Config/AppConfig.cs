using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMDataParser.Config
{
    internal class AppConfig
    {
        public string inputPath;
        public string outputPath;
        public string odtNoGestionados;
        public string logPath;
        public string inputFileName;

        public AppConfig() {

            /*
             * Rutas pruebas
             */
            //inputPath = @"C:\Users\Jay\Desktop\Diners\4 TicketParser ServiceManagerHelix\input\";
<<<<<<< HEAD
            //outputPath = @"C:\Users\Jay\Desktop\Diners\4 TicketParser ServiceManagerHelix\output\";
            //logPath = Path.Combine(@"C:\Users\Jay\Desktop\Diners\4 TicketParser ServiceManagerHelix\input\",
            //              new string($@"{DateTime.Now:yyyy-M-d}\"));
            //odtNoGestionados = Path.Combine(logPath, new string($@"ODTNoGestionados_{DateTime.Now:yyyy-M-d_HH}.csv"));
=======

            //outputPath = @"C:\Users\Jay\Desktop\Diners\4 TicketParser ServiceManagerHelix\output\";

            //logPath = Path.Combine(@"C:\Users\Jay\Desktop\Diners\4 TicketParser ServiceManagerHelix\input\",
            //              new string($@"{DateTime.Now:yyyy-M-d}\"));

            //odtNoGestionados = Path.Combine(logPath, new string($@"ODTNoGestionados_{DateTime.Now:yyyy-M-d_HHmm}.csv"));
>>>>>>> 8fdd8b8cf6c6a2ab79119bd3a63fd79fa57ef6f8

            /*
             * Rutas produccion
            */

            inputPath = @"E:\RECURSOS ROBOT\DATA\MESA_SERVICIO\GESTIONDEUSUARIOS\ARCHBASE\";
<<<<<<< HEAD
            outputPath = @"E:\RECURSOS ROBOT\DATA\MESA_SERVICIO\GESTIONDEUSUARIOS\ARCHBASE\";
            logPath = Path.Combine(@"E:\RECURSOS ROBOT\LOGS\MESA_SERVICIO\GESTIONDEUSUARIOS\",
                        new string($@"{DateTime.Now:yyyy-M-d}\"));
            odtNoGestionados = Path.Combine(logPath, new string($@"ODTNoGestionados_{DateTime.Now:yyyy-M-d_HH}.csv"));

            inputFileName = "export.csv";

=======

            outputPath = @"E:\RECURSOS ROBOT\DATA\MESA_SERVICIO\GESTIONDEUSUARIOS\ARCHBASE\";

            logPath = Path.Combine(@"E:\RECURSOS ROBOT\LOGS\MESA_SERVICIO\GESTIONDEUSUARIOS\",
                        new string($@"{DateTime.Now:yyyy-M-d}\"));
            
            odtNoGestionados = Path.Combine(logPath, new string($@"ODTNoGestionados_{DateTime.Now:yyyy-M-d_HH}.csv"));

            inputFileName = "export.csv";
>>>>>>> 8fdd8b8cf6c6a2ab79119bd3a63fd79fa57ef6f8
        }


        public List<String> estandardInput = new() {
            "accion",
            "identificacion",
            "perfil a asignar",
            "usuario",
            "nombres",
            "correo"
        };

        public List<String> cabeceraFinal = new() { 
            "idodt", 
            "operacion", 
            "nombres apellidos", 
            "identificacion", 
            "correo", 
            "perfil",
            "opcionSistema",
            "usuario", 
            "idpeticionhelix" 
        };


        public void configureLog()
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
