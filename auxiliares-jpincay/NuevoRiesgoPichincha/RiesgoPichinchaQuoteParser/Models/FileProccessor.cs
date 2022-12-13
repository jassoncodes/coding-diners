using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RiesgoPichinchaQuoteParser.Models
{
        public class FileProccessor
    {
        //public string inputPath = "E:\\NUEVO RIESGO PICHINCHA\\Exportaciones.ILB\\";
        public string inputPath = "C:\\Users\\Jay\\Desktop\\Diners\\5 NuevoRiesgoPichincha\\Exportaciones.ILB\\";

        //public string outputPath = "E:\\NUEVO RIESGO PICHINCHA\\Archivos fuente.ILB\\";
        public string outputPath = "C:\\Users\\Jay\\Desktop\\Diners\\5 NuevoRiesgoPichincha\\Archivos fuente.ILB\\";

        //public string logPath = "E:\\RECURSOS ROBOT\\LOGS\\NUEVORIESGO_CTLINT\\";
        public string logPath = "C:\\Users\\Jay\\Desktop\\Diners\\5 NuevoRiesgoPichincha\\Archivos fuente.ILB\\";


        public void ReadFiles(string directoryPath)
        {

            try {

                Log.Information($"Leyendo archivos en directorio: [{directoryPath}]...");
                
                List<RiesgoFiles> filesList = new List<RiesgoFiles>();
                
                // Get all text files in the specified directory
                string[] files = Directory.GetFiles(directoryPath, "*.DEL", SearchOption.AllDirectories);
                
                Log.Information($"{files.LongLength.ToString()} archivos encontrados... ");

                foreach (string file in files)
                {
                    ArrayList fileArrayList = new ArrayList();
                    Log.Information($"Procesando archivo: {file}");

                    //obteniendo nombre de archivo
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    Log.Information($"\tConfigurando nuevo nombre de archivo {fileName}");

                    //configurando nuevo nombre de archivo
                    if (fileName.Contains("PICHINCHA_L"))
                    {
                        fileName = fileName.Replace("PICHINCHA_L", "").Trim();
                    }
                    else if (fileName.Contains("_L"))
                    {
                        fileName = fileName.Replace("_L", "").Trim();
                    }


                    Log.Information($"\tLeyendo contenido de archivo: {file}");
                    //aproach 3
                    using (StreamReader fileReader = new StreamReader(file))
                    {
                        int counter = 0;
                        string ln;
                        while ((ln = fileReader.ReadLine())!= null)
                        {
                            ln = ln.Replace(@"""", String.Empty).Trim();
                            fileArrayList.Add(ln);
                            counter++;
                        }

                        filesList.Add(new RiesgoFiles(fileName, fileArrayList, this.outputPath));
                    }

                    Log.Information($"{filesList.Count} archivos procesados...");
                    
                    
                }
                
                WriteFilesProccessed(filesList);


            }
            catch (Exception e){
                Log.Error(e.ToString());
            }


        }

        private void WriteFile(RiesgoFiles file)
        {
            try
            {

                Log.Information($"Creando archivo txt {file.fullOutPathName}");

                using (TextWriter tw = new StreamWriter(file.fullOutPathName, true))
                {

                    for(int i = 0;i < file.textContent.Count; i++)
                    {
                        tw.WriteLine(file.textContent[i]);
                    }
                }

                Log.Information("Proceso terminado...");
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

        }

        private void WriteFilesProccessed(List<RiesgoFiles> filesList)
        {
            try {
                foreach(RiesgoFiles file in filesList)
                {
                    WriteFile(file);
                }
                Log.Information("Proceso terminado...");
            }
            catch(Exception e) {
                Log.Error(e.ToString());
            }

        }

    }
}
