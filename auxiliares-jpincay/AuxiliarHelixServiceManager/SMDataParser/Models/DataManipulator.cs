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
        private string ExtractEmail(string str)
        {
            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = System.Text.RegularExpressions.Regex.Matches(str, RegexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string matchedEmail = matches[0].Value;

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

                //recibe como parametro una ruta al archivo csv
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(FileManager.ValidarArchivo(filePath));

                //recorre cada hoja
                foreach (Excel._Worksheet sheet in xlWorkbook.Worksheets)
                {
                    //recorre las filas desde la fila 2
                    for (int i = 2; i < sheet.UsedRange.Rows.Count; i++)
                    {

                        //obtiene cadena completa
                        string cadena = sheet.Cells[i, 1].Value2.ToString().ToLower();

                        //quita comillas
                        cadena.Replace(@"""", string.Empty);

                        string rf = cadena.Split(';')[0];

                        cadena = cadena.Split(';')[1];

                        string correo = ExtractEmail(cadena);

                        //remueve tildes y caracteres especiales
                        string c1 = NormalizeString(cadena.Substring(0, cadena.IndexOf(correo)));
                        string c2 = cadena.Substring(cadena.IndexOf(correo));

                        cadena = string.Concat($"{rf};", c1, c2);

                        data.Add(cadena);

                    }
                }
                xlWorkbook.Close(false);
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

            Log.Information($"\tData recibida: {data}");
            Log.Information($"\tData resultado: {compareData}");

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

            //recibe RF01269370;Acción: C Identificación: TCS566395 Perfil a asignar CAO: PERFIL 6 Usuario: Nombres:ZAMBRANO PULUPA JOHANNA ESTEFANIA Correo: jzambrap@pichincha.com	

            try
            {

                foreach (string item in dataList)
                {

                    int c = 0;


                    Estandar dataEstandar = new Estandar();

                    //guarda numero rf
                    dataEstandar.idot = item.Substring(0, item.IndexOf(";"));

                    string dataItem = item.Split(";")[1];

                    foreach (string e in standart)
                    {
                        if (dataItem.Contains(e))
                            c++;
                    }

                    if (c == standart.Count)
                    {
                        if (dataItem.Contains("accion"))
                        {
                            string accion = GetDataBetween(dataItem, "accion", "identificacion");
                            if (accion == "b")
                                dataEstandar.operacion = "borrar";
                            if (accion == "c")
                                dataEstandar.operacion = "crear";
                            if (accion == "a")
                                dataEstandar.operacion = "modificar";
                        }

                        if (dataItem.Contains("perfil a asignar"))
                        {
                            dataEstandar.perfil = GetDataBetween(dataItem, "perfil a asignar", "usuario");
                        }

                        if (dataItem.Contains("usuario"))
                        {
                            dataEstandar.usuario = GetDataBetween(dataItem, "usuario", "nombres");
                        }

                        if (dataItem.Contains("identificacion"))
                        {
                            dataEstandar.identificacion = GetDataBetween(dataItem, "identificacion", "perfil a asignar");
                        }

                        if (dataItem.Contains("nombres"))
                        {
                            dataEstandar.nombres = GetDataBetween(dataItem, "nombres", "correo");
                        }

                        if (dataItem.Contains("correo"))
                        {
                            dataEstandar.correo = ExtractEmail(dataItem);
                        }

                    }

                    //dataEstandar.EstandarParsed();
                    datToWrite.Add(dataEstandar);

                }

            }
            catch (Exception e)
            {
                Log.Error($"Error en el procesamiento de los datos: {e.ToString()}");
            }

            return datToWrite;

        }

    }

}
