using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

public class ExcelSplitter
{
    private static readonly string[] UniqueIDs = { "1030", "1130", "1230", "1430", "1530" };

    public static void SplitExcelFile(string sourceFilePath, int clientsPerFile, string outputDirectory)
    {
        // Create Excel application object
        Excel.Application excelApp = new Excel.Application();
        Excel.Workbook sourceWorkbook = null;

        try
        {
            // Open the source Excel file
            sourceWorkbook = excelApp.Workbooks.Open(sourceFilePath);
            Excel.Worksheet sourceWorksheet = sourceWorkbook.ActiveSheet;

            int totalRecords = sourceWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

            // Determine the number of unique clients in the source worksheet
            HashSet<string> uniqueClients = new HashSet<string>();
            for (int i = 2; i <= totalRecords; i++) // Assuming client ID column is in column B (index 2)
            {
                string clientID = sourceWorksheet.Cells[i, 2].Value?.ToString();
                if (!string.IsNullOrEmpty(clientID))
                {
                    uniqueClients.Add(clientID);
                }
            }

            int clientCount = uniqueClients.Count;
            int filesPerDay = 5;
            int filesPerID = 6;
            int daysPerIncrement = 1;
            int totalFiles = filesPerID * UniqueIDs.Length;
            int currentClient = 0;
            DateTime currentDate = DateTime.Now.Date;

            for (int i = 0; i < totalFiles; i++)
            {
                string uniqueID = UniqueIDs[i % UniqueIDs.Length];
                string fileName = GetUniqueFileName(currentDate, uniqueID);
                string filePath = Path.Combine(outputDirectory, fileName);

                // Create a new workbook for each split file
                Excel.Workbook splitWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet splitWorksheet = splitWorkbook.ActiveSheet;

                int clientsToWrite = Math.Min(clientsPerFile, clientCount - currentClient);

                for (int j = currentClient; j < currentClient + clientsToWrite; j++)
                {
                    string clientID = uniqueClients.ElementAt(j);

                    // Copy the records for the current client from the source worksheet to the split worksheet
                    for (int k = 2; k <= totalRecords; k++) // Assuming client ID column is in column B (index 2)
                    {
                        string currentClientID = sourceWorksheet.Cells[k, 2].Value?.ToString();
                        if (currentClientID == clientID)
                        {
                            Excel.Range sourceRange = sourceWorksheet.Range["A" + k.ToString(), "Z" + k.ToString()];
                            Excel.Range destinationRange = splitWorksheet.Cells[splitWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row + 1, 1];
                            sourceRange.Copy(destinationRange);
                        }
                    }
                }

                // Save the split file with a unique name in the specified output directory
                splitWorkbook.SaveAs(filePath);

                // Close the split workbook
                splitWorkbook.Close();

                currentClient += clientsToWrite;

                // Increment the current date every 5 files
                if ((i + 1) % filesPerDay == 0)
                {
                    currentDate = currentDate.AddDays(daysPerIncrement);
                }
            }

            Console.WriteLine("Excel file split successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred during Excel file splitting: " + ex.Message);
        }
        finally
        {
            // Close and release the Excel objects
            if (sourceWorkbook != null)
            {
                sourceWorkbook.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceWorkbook);
            }

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
    }

    private static string GetUniqueFileName(DateTime date, string uniqueID)
    {
        string dateFormatted = date.ToString("yyyyMMdd");
        return $"{dateFormatted}_{uniqueID}.xlsx";
    }
}
