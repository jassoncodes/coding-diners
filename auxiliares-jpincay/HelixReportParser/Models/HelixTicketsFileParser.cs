using HelixTicket = HelixTicketsReportParser.Models.HelixTicket;
using HelixTicketsReportParser.Models;
using Excel = Microsoft.Office.Interop.Excel;
using Serilog;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace HelixTicketsReportParser.Models
{
    internal class HelixTicketsFileParser
    {


        public List<HelixTicket> GetHelixTickets(Excel.Worksheet sheet)
        {
            List<HelixTicket> helixTickets = new();

            try
            {

                //leer worksheet, crear ticket y agregarlo a la lista a retornar
                for (int row = 1; row <= sheet.UsedRange.Rows.Count; row++)
                {
                    HelixTicket ticket = new();

                    ticket.idWo = sheet.Cells[row, 1].Value2.ToString();
                    ticket.noReq = sheet.Cells[row, 2].Value2.ToString();
                    ticket.idOdt = sheet.Cells[row, 3].Value2.ToString();

                    helixTickets.Add(ticket);
                }


                return helixTickets;

            }
            catch
            (Exception e)
            {
                Log.Error($"GetHelixTickets() Error: No se ha podido obtener la lista de tickets creados\n" +
                    $"Exception: {e}\n{e.Data}");
                return null;
            }


        }

        public List<HelixTicket> GetHelixTicketsList(string inputFileFullPath, string inputFileName)
        {

            try
            {

                Excel.Application excel = new()
                {
                    Visible = false
                };

                Excel.Workbook workbook = excel.Workbooks.Open(
                    FileManager.ValidateInputFilePath(inputFileFullPath, inputFileName),
                    Missing.Value,
                    Missing.Value,
                    Excel.XlFileFormat.xlCSV,
                    Missing.Value,
                    Missing.Value,
                    Missing.Value,
                    Missing.Value,
                    ",",
                    Missing.Value,
                    Missing.Value,
                    Missing.Value,
                    Missing.Value,
                    Missing.Value,
                    Missing.Value
                    );
                Excel.Worksheet worksheet = workbook.Worksheets.Item[1];

                List<HelixTicket> helixTickets = GetHelixTickets(worksheet);

                workbook.Close(false);

                return helixTickets;

            }
            catch (Exception e)
            {
                Log.Error($"GetHelixTicketsList({inputFileFullPath},{inputFileName}) Error: No se ha podido obtener la lista de tickets creados\n" +
                        $"Exception: {e}\n");
                return null;
            }

        }

    }
}
