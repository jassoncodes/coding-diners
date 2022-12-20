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

                return recentFileDir = (from f in directory.GetFiles() where f.Name == appConfig.inputFileName orderby f.LastWriteTime descending select f).First().ToString();

            }
            catch (Exception)
            {
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



            try
            {
                //new instance excel app
                Excel.Application xlApp = new()
                {
                    Visible = false
                };

                Log.Information("Instanciando Excel App: " + xlApp.Path.ToString());

                //new workbook
                Workbook xlWorkbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

                Log.Information("Nuevo archivo excel: " + xlWorkbook.Name.ToString());

                //new worksheet
                Worksheet xlWorksheet = (Worksheet)xlWorkbook.Worksheets.get_Item(1);

                Log.Information("Nueva hoja de excel: " + xlWorksheet.Name.ToString());

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
                        if (Estandar.ValidateFieldsComplete(dataToWrite[r]))
                        {
                            var value = dataToWrite[r].GetIndexFieldValue(c - 1).ToUpper();
                            xlWorksheet.Cells[r + 2, c] = value;
                            xlWorksheet.Cells[r + 2, c].NumberFormat = "@";
                        }
                        else
                        {
                            //escribe solo el rf
                            xlWorksheet.Cells[r + 2, 1] = dataToWrite[r].idot.ToUpper();
                        }
                    }

                    Log.Information(dataToWrite[r].LogData());

                }

                Log.Information("Guardando archivo: " + outputPath + "ArchivoFinal.xls");

                xlWorkbook.SaveAs(outputPath + "ArchivoFinal.xls", Excel.XlFileFormat.xlWorkbookNormal);
                xlWorkbook.Close(true);

                proccessHandler.KillExcelProccess();

            }
            catch (Exception e)
            {
                proccessHandler.KillExcelProccess();
                Log.Error(e.ToString());
                throw;
            }

        }

    }
}
