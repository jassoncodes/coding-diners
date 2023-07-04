using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
namespace createReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string archivoExcel = @"D:\repositorio\coding-diners\BusquedaJuicios\Resultado Generatica para historial BJ.xlsx";

            // Crear una instancia de Excel
            Excel.Application excelApp = new Excel.Application();

            // Abrir el archivo Excel
            Excel.Workbook workbook = excelApp.Workbooks.Open(archivoExcel);

            // Obtener la hoja de trabajo activa
            Excel.Worksheet sheet = workbook.ActiveSheet;

            // Leer los datos de la hoja de trabajo
            Excel.Range range = sheet.Range["B3"].CurrentRegion;
            object[,] data = range.Value;

            // Obtener los valores únicos de la lista
            HashSet<Tuple<object, object, object>> valoresUnicos = new HashSet<Tuple<object, object, object>>();
            int rowCount = data.GetLength(0);
            int colCount = data.GetLength(1);

            for (int row = 1; row <= rowCount; row++)
            {
                object valor1 = data[row, 1];
                object valor2 = data[row, 2];
                object valor3 = data[row, 3];

                valoresUnicos.Add(new Tuple<object, object, object>(valor1, valor2, valor3));
                /*
                if (!string.IsNullOrEmpty(valor1.ToString()) && !string.IsNullOrEmpty(valor2.ToString()) && !string.IsNullOrEmpty(valor3.ToString()))
                {
                    valoresUnicos.Add(new Tuple<object, object, object>(valor1, valor2, valor3));
                }*/
            }

            // Escribir los valores en el archivo Excel existente
            int rowIndex = 3;
            foreach (var tupla in valoresUnicos)
            {
                if (!tupla.Item1.ToString().Contains("Cédula"))
                {
                    sheet.Cells[rowIndex, 2].Value = tupla.Item1;
                    sheet.Cells[rowIndex, 3].Value = tupla.Item2;
                    sheet.Cells[rowIndex, 4].Value = tupla.Item3;

                    rowIndex++;
                }
            }

            // Guardar y cerrar el archivo Excel
            workbook.Save();
            workbook.Close();

            // Cerrar la aplicación Excel
            excelApp.Quit();

            Console.WriteLine("Modificación completada.");
            Console.ReadLine();
        }
    }
}
