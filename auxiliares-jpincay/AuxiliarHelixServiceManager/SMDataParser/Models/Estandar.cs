
using Serilog;

namespace SMDataParser.Models
{
    public class Estandar
    {

        public string operacion;
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
            //List<string> dataEstandar = new List<string>();

            //dataEstandar.Add(this.operacion);
            //dataEstandar.Add(this.ticket);
            //dataEstandar.Add(this.perfil);
            //dataEstandar.Add(this.banco);
            //dataEstandar.Add(this.usuario);
            //dataEstandar.Add(this.identificacion);
            //dataEstandar.Add(this.nombres);
            //dataEstandar.Add(this.correo);
            //dataEstandar.Add(this.area);
            //dataEstandar.Add(this.numerorf);
            //dataEstandar.Add(this.estandar);

        }

        public string EstandarParsed()
        {
            List<string> dataEstandar = new List<string>();

            dataEstandar.Add(this.operacion);
            dataEstandar.Add(this.perfil);
            dataEstandar.Add(this.banco);
            dataEstandar.Add(this.usuario);
            dataEstandar.Add(this.identificacion);
            dataEstandar.Add(this.nombres);
            dataEstandar.Add(this.correo);
            dataEstandar.Add(this.area);
            dataEstandar.Add(this.numerorf);
            dataEstandar.Add(this.estandar);


            string dataParsed = string.Join(", ", dataEstandar);

            return dataParsed;

        }

        public string GetIndexFieldValue(int indice)
        {
            List<string> data = new List<string>();

            data.Add(this.operacion);
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
                Console.WriteLine();
                Log.Information($"Escribiendo: {string.Format($"{string.Join(" ", data)}")}");
            }
            else
            {
                Console.WriteLine($" RF: {this.numerorf} ESTANDARD: {this.estandar}");
                Log.Information($"Escribiendo RF: {this.numerorf} ESTANDARD: {this.estandar}");
            }
        }





    }


}
