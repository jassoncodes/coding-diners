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
        
            //this.inputPath = "E:\\NUEVO RIESGO PICHINCHA\\Exportaciones.ILB\\";
            this.inputPath = "C:\\Users\\Jay\\Desktop\\Diners\\4 TicketParser ServiceManagerHelix\\input\\";

            //this.outputPath = "E:\\NUEVO RIESGO PICHINCHA\\Archivos fuente.ILB\\";
            this.outputPath = "C:\\Users\\Jay\\Desktop\\Diners\\4 TicketParser ServiceManagerHelix\\output\\";

            //this.logPath = "E:\\RECURSOS ROBOT\\LOGS\\";
            this.logPath = "C:\\Users\\Jay\\Desktop\\Diners\\4 TicketParser ServiceManagerHelix\\input\\";

            this.inputFileName = "export.csv";

            this.outputFileName = "ArchivoFinal.xls";
        }


        public List<String> estandardInput = new List<string>() {
            "accion",
            "identificacion",
            "perfil a asignar",
            "usuario",
            "nombres",
            "correo"
        };

        public List<String> cabeceraFinal = new List<string>() { 
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
                .WriteTo.File(this.logPath + System.AppDomain.CurrentDomain.FriendlyName + "_" + ".log",
                    rollingInterval: RollingInterval.Hour,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Log configurado...");
        }

    }
}
