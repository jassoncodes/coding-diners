using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Net.WebRequestMethods;

namespace mergeFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"D:\\BusquedaJuicios\\Descargas"; // Ruta del directorio donde se encuentran los archivos Excel

            string outputFilePath = Path.Combine(directoryPath, "historial.xlsx"); // Ruta del archivo de salida

            string[] excelFiles = Directory.GetFiles(directoryPath, "reporte*.xlsx"); // Obtiene todos los archivos que comienzan con "reporte" y tienen extensión ".xlsx"

            if (excelFiles.Length == 0)
            {
                Console.WriteLine("No se encontraron archivos de Excel que coincidan con el patrón especificado.");
                return;
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook outputWorkbook;
            Excel.Worksheet outputSheet;

            if (System.IO.File.Exists(outputFilePath))
            {
                outputWorkbook = excelApp.Workbooks.Open(outputFilePath);
                outputSheet = outputWorkbook.ActiveSheet as Excel.Worksheet;

                // Encuentra la última fila no vacía en la hoja de salida
                int lastRow = outputSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                int rowIndex = lastRow + 1;

                foreach (string filePath in excelFiles)
                {
                    Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                    Excel.Worksheet sheet = workbook.Worksheets[1] as Excel.Worksheet; // Obtén la primera hoja del archivo

                    if (sheet != null)
                    {
                        int lastRowSource = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                        int lastColumnSource = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;

                        for (int row = 2; row <= lastRowSource; row++) // Comenzar desde la fila 2
                        {
                            for (int col = 1; col <= lastColumnSource; col++)
                            {
                                Excel.Range cell = sheet.Cells[row, col];
                                string cellValue = cell.Value != null ? cell.Value.ToString() : "";

                                // Aquí puedes procesar los datos de cada celda según tus necesidades
                                // Por ejemplo, puedes escribir los datos en el archivo de salida
                                // o realizar algún tipo de cálculo o manipulación de los datos

                                Excel.Range outputCell = outputSheet.Cells[rowIndex, col];
                                outputCell.Value = cellValue;
                            }

                            rowIndex++;
                        }
                    }

                    workbook.Close();
                }

                outputWorkbook.Save();
            }
            else
            {
                outputWorkbook = excelApp.Workbooks.Add();
                outputSheet = outputWorkbook.ActiveSheet as Excel.Worksheet;

                int rowIndex = 2; // Comenzar desde la fila 2

                foreach (string filePath in excelFiles)
                {
                    Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                    Excel.Worksheet sheet = workbook.Worksheets[1] as Excel.Worksheet; // Obtén la primera hoja del archivo

                    if (sheet != null)
                    {
                        int lastRow = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                        int lastColumn = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;

                        for (int row = 2; row <= lastRow; row++) // Comenzar desde la fila 2
                        {
                            for (int col = 1; col <= lastColumn; col++)
                            {
                                Excel.Range cell = sheet.Cells[row, col];
                                string cellValue = cell.Value != null ? cell.Value.ToString() : "";

                                // Aquí puedes procesar los datos de cada celda según tus necesidades
                                // Por ejemplo, puedes escribir los datos en el archivo de salida
                                // o realizar algún tipo de cálculo o manipulación de los datos

                                Excel.Range outputCell = outputSheet.Cells[rowIndex, col];
                                outputCell.Value = cellValue;
                            }

                            rowIndex++;
                        }
                    }

                    workbook.Close();
                }

                outputWorkbook.SaveAs(outputFilePath);
            }

            outputWorkbook.Close();

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            Console.WriteLine("Unificación de archivos completada. El archivo de historial se encuentra en: " + outputFilePath);
        }
    }



    }
