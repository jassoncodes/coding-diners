using Serilog;
using Microsoft.Office.Interop.Excel;
using XlDateGiftCard.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;

namespace XlDateGiftCard
{
    internal class XlDateGiftCard
    {
        public XlDateGiftCard() { }

        static void Main(string[] args)
        {
            //// rutas desarrollo
            //string rutaLog = @"C:\Users\Jay\Desktop\Diners\9 XlDateGiftCard\LOGS\";
            //string rutaArchivo = @"C:\Users\Jay\Desktop\Diners\9 XlDateGiftCard\XlDateGiftCard.xlsx";

            //// rutas produccion
            string rutaLog = @"E:\RECURSOS ROBOT\LOGS\CONTROLGIFTCARD_OPCRED\";
            string rutaArchivo = @"E:\RECURSOS ROBOT\DATA\CONTROLGIFTCARD_OPCRED\XlDateGiftCard.xlsx";
            string dia;

            AppLogger.ConfigLog(rutaLog);

            List<string> fechaCeldas = new List<string>()
            {
                new string(DateTime.Now.ToString("yyyy")),
                new string(DateTime.Now.ToString("MM"))

            };

            int diaSemana = (int)DateTime.Now.DayOfWeek;
            
            if( diaSemana == 1 ) {
                dia = DateTime.Now.AddDays(-3).ToString("dd");
                fechaCeldas.Add(dia);
            }
            else
            {
                dia = DateTime.Now.AddDays(-1).ToString("dd");
                fechaCeldas.Add(dia);
            }


            try
            {
                bool archivoGenerado = new XlDateGiftCard().GenerarArchivo(rutaArchivo, fechaCeldas);

                if (archivoGenerado)
                {
                    Log.Information($"Archivo generado correctamente en: {rutaArchivo}");
                }
                else
                {
                    throw new Exception($"No se pudo generar el archivo en: {rutaArchivo}");
                }

            }catch (Exception e)
            {
                Log.Error($"XlDateGiftCard().Main Error: {e}");
            }

            
        }

        public bool GenerarArchivo(string rutaArchivo, List<string> data)
        {

            try
            {
                Log.Information($"Generando archivo en: {rutaArchivo}");

                Excel.Application excel = new()
                {
                    Visible = false,
                    DefaultSaveFormat = Excel.XlFileFormat.xlWorkbookNormal,
                    DisplayAlerts = false
                };

                Excel.Workbook workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

                Excel.Worksheet sheet = workbook.Worksheets.Item[1];

                sheet.Name = "Date";

                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells[1, i + 1].NumberFormat = "@";
                    sheet.Cells[1, i + 1].Value2 = data[i];
                }

                workbook.SaveAs(rutaArchivo);
                workbook.Close();
                excel.Quit();

                return File.Exists(rutaArchivo);

            }
            catch (Exception e)
            {
                Log.Error($"GenerarArchivo() Error: No se pudo generar archivo en: {rutaArchivo}\n{e}");
                return false;

            }
        }

    }
}