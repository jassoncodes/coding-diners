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
        public string logPath;
        public string inputFileName;
        public string outputFileName;


        public AppConfig() { 
        
            this.inputPath = "E:\\RECURSOS ROBOT\\DATA\\SM_HELIX\\MESA_SERVICIO\\GESTIONDEUSUARIOS\\AUXILIAR\\";
            //this.inputPath = "C:\\Users\\Jay\\Desktop\\Diners\\4 TicketParser ServiceManagerHelix\\input\\";

            this.outputPath = "E:\\RECURSOS ROBOT\\DATA\\SM_HELIX\\MESA_SERVICIO\\GESTIONDEUSUARIOS\\ARCHIVOFINAL\\";
            //this.outputPath = "C:\\Users\\Jay\\Desktop\\Diners\\4 TicketParser ServiceManagerHelix\\output\\";

            this.logPath = "E:\\RECURSOS ROBOT\\LOGS\\MESA_SERVICIO\\";
            //this.logPath = "C:\\Users\\Jay\\Desktop\\Diners\\4 TicketParser ServiceManagerHelix\\input\\";

            this.inputFileName = "export.csv";

            this.outputFileName = "ArchivoFinal.xls";
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
            "usuario", 
            "idpeticionhelix" 
        };


        public void configureLog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"{this.logPath}{System.AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:yyyyMMdd-HHmm}.log",
                                 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Log configurado...");
        }

    }
}
