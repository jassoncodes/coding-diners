using AppConfig = SMDataParser.Config.AppConfig;
using System.Text;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using Serilog;

namespace SMDataParser.Models
{
    internal class DataManipulator
    {

        //quita las tildes de una cadena
        static string NormalizeString(string cadena) => Regex.Replace(cadena.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");


        //obtiene email de una cadena
        //public string[] ExtractEmails(string str)
        private string ExtractEmails(string str)
        {
            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = System.Text.RegularExpressions.Regex.Matches(str, RegexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string matchedEmail = matches[matches.Count].Value;

            //string[] MatchList = new string[matches.Count];

            // add each match
            //int c = 0;
            //foreach (System.Text.RegularExpressions.Match match in matches)
            //{
            //    MatchList[c] = match.ToString();
            //    c++;
            //}

            //return MatchList;
            return matchedEmail;
        }


        //lee input y obtiene las tramas a procesar y las devuelve como lista
        public List<string> GetData(string filePath)
        {
            ProccessHandler proccessHandler = new ProccessHandler();
            FileManager fileManager = new FileManager();

            List<String> data = new List<String>();

            try
            {

                Excel.Application xlApp = new Excel.Application();
                xlApp.Visible = false;

                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileManager.ValidarArchivo(filePath));

                foreach (Excel._Worksheet sheet in xlWorkbook.Worksheets)
                {
                    foreach (Excel.Range row in sheet.UsedRange.Rows)
                    {
                        //obtiene cadena descripcion
                        string cadena = sheet.Cells[row.Row, 2].Value2.ToString().ToLower();

                        //string[] emailExtract = ExtractEmails(cadena);
                        string emailExtract = ExtractEmails(cadena);

                        if (emailExtract.Length > 0)
                        {
                            //string correo = emailExtract[0];
                            string correo = emailExtract;
                            string c1 = NormalizeString(cadena.Substring(0, cadena.IndexOf(correo)));
                            string c2 = cadena.Substring(cadena.IndexOf(correo));

                            cadena = string.Concat(c1, c2);

                            //agrega candena a la lista de retorno y concatena valor de campo numero rf
                            data.Add(cadena
                                        + " rf " + sheet.Cells[row.Row, 1].Value2.ToString());
                        }
                        else
                        {
                            data.Add(NormalizeString(cadena)
                                        + " rf " + sheet.Cells[row.Row, 1].Value2.ToString());
                        }

                    }
                }

                proccessHandler.KillExcelProccess();

                return data;

            }
            catch (Exception e)
            {
                Log.Error($"Error: {e.ToString()}");
                proccessHandler.KillExcelProccess();
                throw;

            }
        }

        private string GetDataBetween(string data, string estandar1, string estandar2)
        {
            string value = "";

            int ind1 = data.IndexOf(estandar1) + estandar1.Length;
            int ind2 = data.IndexOf(estandar2, ind1);

            value = data.Substring(ind1, ind2 - ind1).Trim();

            return value;
        }


        private string StringDataToCompare(Estandar dataToString)
        {
            string dataString = "";
            string operacion = "";

            if (dataToString.operacion == "modificar")
                operacion = "a";
            if (dataToString.operacion == "borrar")
                operacion = "b";
            if (dataToString.operacion == "crear")
                operacion = "c";

            List<string> data = new List<string>();

            data.Add(("accion " + operacion).Trim());
            data.Add(("identificacion " + dataToString.identificacion).Trim());
            data.Add(("perfil a asignar " + dataToString.perfil).Trim());
            data.Add(("usuario " + dataToString.usuario).Trim());
            data.Add(("nombres " + dataToString.nombres).Trim());
            data.Add(("correo " + dataToString.correo).Trim());

            dataString = string.Join(" ", data);


            return dataString;
        }


        private bool Compare(Estandar dataEstandar, string data)
        {
            bool compare = false;
            string compareData = StringDataToCompare(dataEstandar);

            if (compareData == data)
            {
                compare = true;
            }

            return compare;
        }


        public List<Estandar> ParseData(List<string> dataList)
        {
            //string[] standart = { "accion", "identificacion", "perfil a asignar", "usuario", "nombres", "correo" };

            List<string> standart = new AppConfig().estandardInput;


            List<Estandar> datToWrite = new List<Estandar>();

            foreach (String item in dataList)
            {

                int c = 0;

                List<string> values = item.Split().ToList();

                Estandar dataEstandar = new Estandar();

                try
                {
                    //guarda ticket
                    //dataEstandar.ticket = values.SkipWhile(x => x != "ticket").Skip(1).DefaultIfEmpty(values[0]).FirstOrDefault();

                    //guarda numero rf
                    dataEstandar.numerorf = values.SkipWhile(x => x != "rf").Skip(1).DefaultIfEmpty(values[0]).FirstOrDefault();


                    foreach (string e in standart)
                    {
                        if (item.Contains(e))
                            c++;
                    }

                    if (c == standart.Count)
                    {
                        if (item.Contains("accion"))
                        {
                            string accion = GetDataBetween(item, "accion", "identificacion");
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
                            dataEstandar.perfil = GetDataBetween(item, "perfil a asignar", "usuario");
                            c++;
                        }

                        if (item.Contains("usuario"))
                        {
                            dataEstandar.usuario = GetDataBetween(item, "usuario", "nombres");
                            c++;
                        }

                        if (item.Contains("identificacion"))
                        {
                            dataEstandar.identificacion = GetDataBetween(item, "identificacion", "perfil a asignar");
                            c++;
                        }

                        if (item.Contains("nombres"))
                        {
                            dataEstandar.nombres = GetDataBetween(item, "nombres", "correo");
                            c++;
                        }

                        if (item.Contains("correo"))
                        {
                            dataEstandar.correo = GetDataBetween(item, "correo", "rf");
                            c++;
                        }

                        if (!Compare(dataEstandar, item.Substring(0, item.IndexOf(" rf "))))
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


                    Log.Information($"Data procesada y agregada para escritura: {dataEstandar.EstandarParsed}");
                    datToWrite.Add(dataEstandar);

                }
                catch (Exception e)
                {
                    Log.Error($"Error en el procesamiento de los datos: {e.ToString()}");
                }


            }

            return datToWrite;

        }


        public int ContarEstandarSi(List<Estandar> lista)
        {
            int c = 0;

            lista.ForEach(e =>
            {
                if (e.estandar == "SI")
                    c++;
            });

            return c;
        }


        public int ContarEstandarNo(List<Estandar> lista)
        {
            int c = 0;
            lista.ForEach(e =>
            {
                if (e.estandar != "SI")
                    c++;
            });
            return c;
        }



    }

}
