using Microsoft.Office.Interop.Excel;
using Serilog;
using Excel = Microsoft.Office.Interop.Excel;
using AppConfig = SMDataParser.Config.AppConfig;

namespace SMDataParser.Models
{
    internal class FileManager
    {
        ProccessHandler proccessHandler = new ProccessHandler();

        //busca archivo, valida nomeclatura de nombre, devuelve archivo más reciente
        public string ValidarArchivo(String path)
        {
            AppConfig appConfig = new AppConfig();
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
                Excel.Application xlApp = new Excel.Application();
                xlApp.Visible = false;

                Log.Information("Instanciando Excel App: " + xlApp.Path.ToString());

                //new workbook
                Workbook xlWorkbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

                Log.Information("Nuevo archivo excel: " + xlWorkbook.Name.ToString());

                //new worksheet
                Worksheet xlWorksheet = (Worksheet)xlWorkbook.Worksheets.get_Item(1);

                Log.Information("Nueva hoja de excel: " + xlWorksheet.Name.ToString()); ;

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
                        if (dataToWrite[r].estandar == "SI")
                        {
                            var value = dataToWrite[r].GetIndexFieldValue(c - 1).ToUpper();
                            xlWorksheet.Cells[r + 2, c] = value;
                            xlWorksheet.Cells[r + 2, c].NumberFormat = "@";
                        }
                        else
                        {
                            //xlWorksheet.Cells[r + 2, 2] = dataToWrite[r].ticket;
                            //xlWorksheet.Cells[r + 2, 2].NumberFormat = "@";

                            xlWorksheet.Cells[r + 2, 10] = dataToWrite[r].numerorf;
                            xlWorksheet.Cells[r + 2, 10].NumberFormat = "@";

                            xlWorksheet.Cells[r + 2, 11] = dataToWrite[r].estandar;
                            xlWorksheet.Cells[r + 2, 11].NumberFormat = "@";
                        }
                    }
                    dataToWrite[r].PrintDataEstandar();
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
