using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMDataParser.Models
{
    internal class DataManipulator
    {
        //lee input y obtiene las tramas a procesar y las devuelve como lista
        private List<string> GetData(string filePath)
        {
            KillProcess k = new KillProcess();


            List<String> data = new List<String>();

            try
            {

                Excel.Application xlApp = new Excel.Application();
                xlApp.Visible = false;

                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ValidarArchivo(filePath));

                foreach (Excel._Worksheet sheet in xlWorkbook.Worksheets)
                {
                    foreach (Excel.Range row in sheet.UsedRange.Rows)
                    {

                        string cadena = sheet.Cells[row.Row, 3].Value2.ToString().ToLower();

                        string[] emailExtract = ExtractEmails(cadena);

                        if (emailExtract.Length > 0)
                        {
                            string correo = emailExtract[0];
                            string c1 = NormalizeString(cadena.Substring(0, cadena.IndexOf(correo)));
                            string c2 = cadena.Substring(cadena.IndexOf(correo));

                            cadena = string.Concat(c1, c2);
                            data.Add(cadena
                                        + " rf " + sheet.Cells[row.Row, 1].Value2.ToString()
                                        + " ticket " + sheet.Cells[row.Row, 2].Value2.ToString());
                        }
                        else
                        {
                            data.Add(NormalizeString(cadena)
                                        + " rf " + sheet.Cells[row.Row, 1].Value2.ToString()
                                        + " ticket " + sheet.Cells[row.Row, 2].Value2.ToString());
                        }

                    }
                }

                KillProcess.KillExcelProccess();

                return data;

            }
            catch (Exception)
            {
                KillProcess.KillExcelProccess();
                throw;

            }
        }


    }

}
}
