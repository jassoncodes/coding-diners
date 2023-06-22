using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Net.WebRequestMethods;
using Microsoft.Office.Interop.Excel;

namespace mergeFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"E:\RECURSOS ROBOT\DATA\BUSQUEDAJUICIOS\ARCHIVOS\CasosIncidenciaHistorial"; // Ruta del directorio donde se encuentran los archivos Excel

            string outputFilePath = Path.Combine(directoryPath, "ConsolidadoHistoriales.xlsx"); // Ruta del archivo de salida

            string[] excelFiles = Directory.GetFiles(directoryPath, "CasoIncidencia*.xls"); // Obtiene todos los archivos que comienzan con "reporte" y tienen extensión ".xlsx"
            
            int rowIndex = 0;

            if (excelFiles.Length == 0)
            {
                Console.WriteLine("No se encontraron archivos de Excel que coincidan con el patrón especificado.");
                return;
            }

            Excel.Application excelApp = new Excel.Application()
            {
                Visible = false,
                DisplayAlerts = false
            };

            Excel.Workbook outputWorkbook;
            Excel.Worksheet outputSheet;
            bool OutputFileExist = System.IO.File.Exists(outputFilePath);

            if (OutputFileExist)
            {
                outputWorkbook = excelApp.Workbooks.Open(outputFilePath);
                outputSheet = outputWorkbook.ActiveSheet as Excel.Worksheet;

                // Encuentra la última fila no vacía en la hoja de salida
                int lastRow = outputSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                rowIndex = lastRow + 1;

            }
            else
            {
                //System.IO.File.Create(outputFilePath);
                //outputWorkbook = excelApp.Workbooks.Open(outputFilePath);
                outputWorkbook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                outputSheet = outputWorkbook.ActiveSheet as Excel.Worksheet;

                //copiando cabecera
                Excel.Workbook Wbk;
                Excel.Worksheet Sht;

                string CabSource = excelFiles[0];

                Wbk = excelApp.Workbooks.Open(CabSource);
                Sht = Wbk.Worksheets[1] as Excel.Worksheet;

                for(int col = 1; col <= Sht.UsedRange.Columns.Count; col++)
                {
                    Excel.Range cabCellSource = Sht.Cells[5, col];
                    string cabValue = cabCellSource.Value != null ? Convert.ToString(cabCellSource.Value2) : "";

                    Excel.Range cabOuputCell = outputSheet.Cells[1, col];
                    cabOuputCell.Value = cabValue;
                    cabOuputCell.NumberFormat = "@";

                }

                Wbk.Close();

            }
            
            rowIndex = 2;
            foreach (string filePath in excelFiles)
            {
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet sheet = workbook.Worksheets[1] as Excel.Worksheet; // Obtén la primera hoja del archivo

                if (sheet != null)
                {
                    int lastRow = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                    int lastColumn = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;

                    for (int row = 6; row <= lastRow; row++) // Comenzar desde la fila 4 (jpincay)
                                                             //for (int row = 2; row <= lastRow; row++) // Comenzar desde la fila 2
                    {
                        for (int col = 1; col <= lastColumn; col++)
                        {
                            Excel.Range cell = sheet.Cells[row, col];
                            string cellValue = cell.Value != null ? Convert.ToString(cell.Value2) : "";

                            // Aquí puedes procesar los datos de cada celda según tus necesidades
                            // Por ejemplo, puedes escribir los datos en el archivo de salida
                            // o realizar algún tipo de cálculo o manipulación de los datos

                            Excel.Range outputCell = outputSheet.Cells[rowIndex, col];
                            outputCell.Value = cellValue;
                            outputCell.NumberFormat = "@";
                            outputCell.WrapText = false;
                        }

                        rowIndex++;
                    }
                }

                workbook.Close();
            }

            outputWorkbook.SaveAs(outputFilePath);

            outputWorkbook.Close();

            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            Console.WriteLine("Unificación de archivos completada. El archivo de historial se encuentra en: " + outputFilePath);
        }
    }



    }
