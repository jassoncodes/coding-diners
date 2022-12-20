
using Serilog;

namespace SMDataParser.Models
{
    public class Estandar
    {

        public string operacion;
        public string ticket;
        public string perfil;
        public string banco = "PICHINCHA";
        public string usuario;
        public string identificacion;
        public string nombres;
        public string correo;
        public string area = "CT";
        public string numerorf;
        public string estandar;

        public Estandar()
        {
            List<string> dataEstandar = new List<string>();

            dataEstandar.Add(this.operacion);
            dataEstandar.Add(this.ticket);
            dataEstandar.Add(this.perfil);
            dataEstandar.Add(this.banco);
            dataEstandar.Add(this.usuario);
            dataEstandar.Add(this.identificacion);
            dataEstandar.Add(this.nombres);
            dataEstandar.Add(this.correo);
            dataEstandar.Add(this.area);
            dataEstandar.Add(this.numerorf);
            dataEstandar.Add(this.estandar);

        }

        public string GetDataBetween(string data, string estandar1, string estandar2)
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


        public bool Compare(Estandar dataEstandar, string data)
        {
            bool compare = false;
            string compareData = StringDataToCompare(dataEstandar);

            if (compareData == data)
            {
                compare = true;
            }

            return compare;
        }

        public string GetIndexFieldValue(int indice)
        {
            List<string> data = new List<string>();

            data.Add(this.operacion);
            data.Add(this.ticket);
            data.Add(this.perfil);
            data.Add(this.banco);
            data.Add(this.usuario);
            data.Add(this.identificacion);
            data.Add(this.nombres);
            data.Add(this.correo);
            data.Add(this.area);
            data.Add(this.numerorf);
            data.Add(this.estandar);

            return data[indice];

        }

        public void PrintDataEstandar()
        {
            List<string> data = new List<string>();
            data.Add(this.operacion);
            data.Add(this.ticket);
            data.Add(this.perfil);
            data.Add(this.banco);
            data.Add(this.usuario);
            data.Add(this.identificacion);
            data.Add(this.nombres);
            data.Add(this.correo);
            data.Add(this.area);
            data.Add(this.numerorf);
            data.Add(this.estandar);

            if (this.estandar == "SI")
            {
                ///data.ForEach(e => { Console.WriteLine(e.ToString()); });
                Console.WriteLine(string.Format("{0}", string.Join(" ", data)));
                Log.Information("Escribiendo: " + string.Format("{0}", string.Join(" ", data)));
            }
            else
            {
                Console.WriteLine("{0} {1} {2}", this.numerorf, this.ticket, this.estandar);
                Log.Information("Escribiendo: " + "{0} {1} {2}", this.numerorf, this.ticket, this.estandar);
            }
        }





    }


}
