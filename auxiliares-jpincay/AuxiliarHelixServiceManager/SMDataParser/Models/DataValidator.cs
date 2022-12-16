﻿using System.Text.RegularExpressions;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Serilog;
using System.Collections.Generic;

namespace SMDataParser.Models
{

    //class KillProcess{

    //    //kill excel process
    //    public void KillExcelProccess()
    //    {
    //        foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
    //        {
    //            proc.Kill();
    //        }
    //    }

    //}

    class DataValidator
    {

        public KillProcess KillProcess;

        public String inputPath = "C:\\Users\\Jay\\Desktop\\Diners\\3 StandartValidator Test Files\\input\\";
        //public String inputPath = "E:\\RECURSOS ROBOT\\DATA\\MESA_SERVICIO\\GESTIONDEUSUARIOS\\AUXILIAR\\";

        public String outputPath = "C:\\Users\\Jay\\Desktop\\Diners\\3 StandartValidator Test Files\\output";
        //public String outputPath = "E:\\RECURSOS ROBOT\\DATA\\MESA_SERVICIO\\GESTIONDEUSUARIOS\\ARCHIVOFINAL\\";

        public List<String> cabeceraFinal = new List<string>() { "operacion", "ticket", "perfil", "banco", "usuario", "identificacion", "nombres apellidos", "correo", "area", "numero", "estandar" };

        //quita las tildes de una cadena
        static string NormalizeString(string cadena) => Regex.Replace(cadena.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");


        //obtiene email de una cadena
        public string[] ExtractEmails(string str)
        {
            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = System.Text.RegularExpressions.Regex.Matches(str, RegexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // add each match
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }

            return MatchList;
        }





    class DataParser
    {


        public KillProcess KillProcess;

        public List<Estandar> ParseData(List<string> dataList)
        {
            string[] standart = { "accion", "identificacion", "perfil a asignar", "usuario", "nombres", "correo" };
            List<Estandar> datToWrite = new List<Estandar>();

            foreach (String item in dataList)
            {

                int c = 0;

                List<string> values = item.Split().ToList();

                Estandar dataEstandar = new Estandar();

                //guarda ticket
                dataEstandar.ticket = values.SkipWhile(x => x != "ticket").Skip(1).DefaultIfEmpty(values[0]).FirstOrDefault();

                //guarda numero rf
                dataEstandar.numerorf = values.SkipWhile(x => x != "rf").Skip(1).DefaultIfEmpty(values[0]).FirstOrDefault();


                foreach (string e in standart)
                {
                    if (item.Contains(e))
                        c++;
                }

                if (c == standart.Length)
                {
                    if (item.Contains("accion"))
                    {
                        string accion = dataEstandar.GetDataBetween(item, "accion", "identificacion");
                        if (accion == "b")
                            dataEstandar.operacion = "borrar";
                        if (accion == "c")
                            dataEstandar.operacion = "crear";
                        if (accion == "a")
                            dataEstandar.operacion = "modificar";
                        c++;
                    }

                    if (item.Contains("perfil a asignar"))
                    {
                        dataEstandar.perfil = dataEstandar.GetDataBetween(item, "perfil a asignar", "usuario");
                        c++;
                    }

                    if (item.Contains("usuario"))
                    {
                        dataEstandar.usuario = dataEstandar.GetDataBetween(item, "usuario", "nombres");
                        c++;
                    }

                    if (item.Contains("identificacion"))
                    {
                        dataEstandar.identificacion = dataEstandar.GetDataBetween(item, "identificacion", "perfil a asignar");
                        c++;
                    }

                    if (item.Contains("nombres"))
                    {
                        dataEstandar.nombres = dataEstandar.GetDataBetween(item, "nombres", "correo");
                        c++;
                    }

                    if (item.Contains("correo"))
                    {
                        dataEstandar.correo = dataEstandar.GetDataBetween(item, "correo", "rf");
                        c++;
                    }

                    if (!dataEstandar.Compare(dataEstandar, item.Substring(0, item.IndexOf(" rf "))))
                    {
                        dataEstandar.estandar = "MANUAL";
                    }
                    else
                    {
                        dataEstandar.estandar = "SI";
                    }
                }
                else
                {
                    dataEstandar.estandar = "MANUAL";
                }



                datToWrite.Add(dataEstandar);

            }

            return datToWrite;

        }

        public void WriteFile(List<Estandar> dataToWrite)
        {
            String outPath = new DataValidator().outputPath;
            List<String> cabeceraFinal = new DataValidator().cabeceraFinal;

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
                            xlWorksheet.Cells[r + 2, 2] = dataToWrite[r].ticket;
                            xlWorksheet.Cells[r + 2, 2].NumberFormat = "@";

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
                KillProcess.KillExcelProccess();


            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                throw;
                KillProcess.KillExcelProccess();
            }


        }


    }



}
