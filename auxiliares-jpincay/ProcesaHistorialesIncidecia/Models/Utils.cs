using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcesaHistorialesIncidecia.Models
{
    internal class Utils
    {
        public static string NormalizeString(string cadena) => Regex.Replace(cadena.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");

        public static bool VerificarDiferenciaAniosMayorDos(string fechaString)
        {
            if (DateTime.TryParseExact(fechaString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
            {
                DateTime fechaActual = DateTime.Now;

                int diferenciaAnios = fechaActual.Year - fecha.Year;

                if (fechaActual.Month < fecha.Month || (fechaActual.Month == fecha.Month && fechaActual.Day < fecha.Day))
                {
                    diferenciaAnios--;
                }

                return diferenciaAnios >= 2;
            }

            return false;
        }

        public static string SumarDias(int cantidadDias)
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaSumada = fechaActual.AddDays(cantidadDias);
            string fechaFormateada = fechaSumada.ToString("dd/MM/yyyy");
            return fechaFormateada;
        }

        public static void ComprimirArchivos(string nombreArchivoZip, params string[] rutasArchivos)
        {
            string rutaCompleta = Path.GetFullPath(nombreArchivoZip);

            using (ZipArchive zip = ZipFile.Open(rutaCompleta, ZipArchiveMode.Create))
            {
                foreach (string rutaArchivo in rutasArchivos)
                {
                    string nombreArchivo = Path.GetFileName(rutaArchivo);
                    zip.CreateEntryFromFile(rutaArchivo, nombreArchivo);
                }
            }

            Log.Information($"Archivos comprimidos correctamente en: {rutaCompleta}");
        }
    }
}
