using Microsoft.Office.Interop.Excel;
using Serilog;
using Excel = Microsoft.Office.Interop.Excel;
using AppConfig = SMDataParser.Config.AppConfig;

namespace SMDataParser.Models
{
    internal class FileManager
    {

        readonly ProccessHandler proccessHandler = new();

        //busca archivo, valida nomeclatura de nombre, devuelve archivo más reciente
        public static string ValidarArchivo(String path)
        {
            AppConfig appConfig = new();
            string recentFileDir = "";

            try
            {

                //Lee directorio en busqueda de archivo mas reciente
                var directory = new DirectoryInfo(path);

                Log.Information($"ValidarArchivo(): Leyendo directorio {directory} en busca de {appConfig.inputPath}...");

                recentFileDir = (from f in directory.GetFiles() where f.Name == appConfig.inputFileName orderby f.LastWriteTime descending select f).First().ToString();

                Log.Information($"ValidarArchivo(): Archivo encontrado: {recentFileDir}");

                return recentFileDir;

            }
            catch (Exception e)
            {
                Log.Error($"ValidarArchivo() Error: Error en la lectura de directorio ({path}) \n" +
                    $"\nError: {e.ToString()}");
                throw;
            }

        }

        public void WriteFile(List<Estandar> dataToWrite)
        {
            String outPath = new AppConfig().outputPath;
            List<String> cabeceraFinal = new AppConfig().cabeceraFinal;

            string folderName = DateTime.Now.ToString("yyyy-M-d");

            string outputPath = outPath + folderName + "\\";

            bool folderOutput = System.IO.Directory.Exists(outputPath);
            if (!folderOutput)
                System.IO.Directory.CreateDirectory(outputPath);

            Log.Information($"Ruta de ArchivoFinal.xls configurada: {outputPath}");

            try
            {
                //new instance excel app
                Excel.Application xlApp = new()
                {
                    Visible = false
                };

                //Log.Information($"WriteFile(): Instanciando Excel App {xlApp.Path.ToString()} ");

                //new workbook
                Workbook xlWorkbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

                //Log.Information($"WriteFile(): Nuevo archivo excel: {xlWorkbook.Name.ToString()}");

                //new worksheet
                Worksheet xlWorksheet = (Worksheet)xlWorkbook.Worksheets.get_Item(1);

                //Log.Information($"WriteFile(): Nueva hoja de excel: {xlWorksheet.Name.ToString()}");

                //escribe cabeceras de columnas
                foreach (String cabecera in cabeceraFinal)
                {
                    xlWorksheet.Cells[1, cabeceraFinal.IndexOf(cabecera) + 1] = cabecera.ToUpper();
                    xlWorksheet.Cells[1, cabeceraFinal.Count].EntireRow.Font.Bold = true;
                }

                //recorrer lista de objetos DataEstandar
                for (int r = 0; r < dataToWrite.Count; r++)
                {
                    for (int c = 1; c < cabeceraFinal.Count; c++)
                    {
                        //if (Estandar.ValidateFieldsComplete(dataToWrite[r]))
                        //{
                            var value = dataToWrite[r].GetIndexFieldValue(c - 1);
                            xlWorksheet.Cells[r + 2, c] = value;
                            xlWorksheet.Cells[r + 2, c].NumberFormat = "@";
                        //}
                        //else
                        //{
                            //escribe solo rf
                            //xlWorksheet.Cells[r + 2, 1] = dataToWrite[r].idot.ToUpper();

                            //escribe en el log
                        //    string logData = dataToWrite[r].idot.ToUpper();
                        //    Log.Information($"\t{logData} no registrado, campos incompletos...");
                        //}
                    }

                    Log.Information(dataToWrite[r].LogData());

                }

                Log.Information($"WriteFile(): Guardando archivo ArchivoFinal.xls en : {outputPath}");

                xlWorkbook.SaveAs(outputPath + "ArchivoFinal.xls", Excel.XlFileFormat.xlWorkbookNormal);
                xlWorkbook.Close(true);

                proccessHandler.KillExcelProccess();


            }
            catch (Exception e)
            {
                proccessHandler.KillExcelProccess();
                Log.Error($"WriteFile(): Error al escribir ArchivoFinal.xls" +
                    $"\nError: {e.ToString()}");
                throw;
            }

        }

        public void DeleteInput(String path)
        {
            Log.Warning($"DeleteInput(): Borrando arhivo input {path}");
            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception e)
            {
                Log.Error($"DeleteInput() Error: \n{e}");
            }
        }

    }
}
