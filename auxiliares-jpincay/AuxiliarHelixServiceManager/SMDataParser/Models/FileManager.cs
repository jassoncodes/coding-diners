using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMDataParser.Models
{
    internal class FileManager
    {
        //busca archivo, valida nomeclatura de nombre, devuelve archivo más reciente
        public string ValidarArchivo(String path)
        {
            string recentFileDir = "";

            try
            {

                //Lee directorio en busqueda de archivo mas reciente
                var directory = new DirectoryInfo(path);
                return recentFileDir = (from f in directory.GetFiles() where f.Name == "ArchivoAux.xls" orderby f.LastWriteTime descending select f).First().ToString();

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
