using Serilog;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RiesgoPichinchaQuoteParser.Models
{
    public class FileTarget {
        public string name;
        public string[] textContent;
        public string outPath;
        public string outFileName;
        public string fullOutPathName;

        public FileTarget(string name, string[] textContent, string path) {
            this.name = name;
            this.textContent = textContent;
            this.outPath = path;
            this.outFileName = name + ".txt";
            this.fullOutPathName = this.outPath + this.outFileName;
        }

        public FileTarget(){
            this.name = "";
            this.textContent = Array.Empty<string>();
            this.outPath = "";
            this.outFileName = "";
            this.fullOutPathName = "";
        }
    
    }
    public class LoggerConfigurator
    {


        FileProccessor fileProccessor = new FileProccessor();

        public void configureLog()
        {
            Log.Information("Configurando log...");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(fileProccessor.logPath + System.AppDomain.CurrentDomain.FriendlyName + "_" + ".log",
                    rollingInterval: RollingInterval.Hour,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Log configurado...");
        }

    }


    public class FileProccessor
    {
        public string inputPath = "E:\\NUEVO RIESGO PICHINCHA\\Exportaciones.ILB\\";
        //public string inputPath = "C:\\Users\\Jay\\Desktop\\Diners\\5 NuevoRiesgoPichincha\\Exportaciones.ILB\\";

        public string outputPath = "E:\\NUEVO RIESGO PICHINCHA\\Archivos fuente.ILB\\";
        //public string outputPath = "C:\\Users\\Jay\\Desktop\\Diners\\5 NuevoRiesgoPichincha\\Archivos fuente.ILB\\";

        public string logPath = "E:\\RECURSOS ROBOT\\LOGS\\NUEVORIESGO_CTLINT\\";
        //public string logPath = "C:\\Users\\Jay\\Desktop\\Diners\\5 NuevoRiesgoPichincha\\Archivos fuente.ILB\\";


        public void ReadFiles(string directoryPath)
        {

            try {

                Log.Information($"Leyendo archivos en directorio: [{directoryPath}]...");
                
                List<FileTarget> filesList = new List<FileTarget>();
                
                // Get all text files in the specified directory
                string[] files = Directory.GetFiles(directoryPath, "*.DEL", SearchOption.AllDirectories);
                
                Log.Information($"{files.LongLength.ToString()} archivos encontrados... ");

                foreach (string file in files)
                {

                    Log.Information($"Leyendo contenido de archivo: {file}");

                    string[] textFromFile = File.ReadAllLines(file);
                    string[] textToFile = new string[textFromFile.Length];

                    for(int i = 0; i < textFromFile.Length; i++)
                    {
                        textToFile[i] = textFromFile[i].Replace(@"""", String.Empty).Trim();
                    }
                    
                    //uniendo las lineas
                    //string textProccessed = string.Join(" ", textFromFile);

                    /* aproach 1
                     * 
                    //Read the file using either ANSI or UTF-8 encoding
                    //string text = File.ReadAllText(file, Encoding.UTF8);

                    //Log.Information($"Reemplazando \"...");
                    // Process the text from the file
                    //text = text.Replace(@"""", String.Empty).Trim();
                    */


                    //obteniendo nombre de archivo
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    Log.Information($"Configurando nuevo nombre de archivo {fileName}");


                    //configurando nuevo nombre de archivo
                    if (fileName.Contains("PICHINCHA_L")){
                        fileName = fileName.Replace("PICHINCHA_L", "").Trim();
                    }
                    else if (fileName.Contains("_L")){
                        fileName = fileName.Replace("_L", "").Trim();
                    }

                    //agrega a lista de archivos a generar
                    //filesList.Add(new FileTarget(fileName,text,this.outputPath));
                    filesList.Add(new FileTarget(fileName, textToFile,this.outputPath));
                    Log.Information($"{filesList.Count} archivos procesados...");
                    
                    
                }
                
                WriteFilesProccessed(filesList);


            }
            catch (Exception e){
                Log.Error(e.ToString());
            }


        }

        private void WriteFile(FileTarget file)
        {
            try
            {

                Log.Information($"Creando archivo txt {file.fullOutPathName}");
                using FileStream fs = File.Create(file.fullOutPathName);
                using var sr = new StreamWriter(fs);
                for(int i = 0;i < file.textContent.Length; i++)
                {
                    sr.WriteLine(file.textContent[i]);
                }

                /*
                 * approach 1
                using (System.IO.FileStream fs = System.IO.File.Create(System.IO.Path.Combine(file.outPath, file.outFileName)))
                {
                    byte[] byteArr = Encoding.ASCII.GetBytes(file.textContent);

                    for (byte i = 0; i < byteArr.Length; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
                */

                Log.Information("Proceso terminado...");
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

        }

        private void WriteFilesProccessed(List<FileTarget> filesList)
        {
            try {
                foreach(FileTarget file in filesList)
                {

                    WriteFile(file);

                    //code in dev-jpincay branch
                    //Log.Information($"Creando archivo txt {file.outFileName}");
                    //System.IO.File.WriteAllText(System.IO.Path.Combine(file.outPath, file.outFileName),file.textContent);

                }
                Log.Information("Proceso terminado...");
            }
            catch(Exception e) {
                Log.Error(e.ToString());
            }

        }

    }
}
