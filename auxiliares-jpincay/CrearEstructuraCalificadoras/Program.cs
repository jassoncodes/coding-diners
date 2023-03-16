using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrearEstructuraCalificadoras;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        List<Carpeta> listaCarpetas = new();

        using(StreamReader r = new StreamReader(".\\params\\estructura.json")){
            string json = r.ReadToEnd();
            listaCarpetas = JsonSerializer.Deserialize<List<Carpeta>>(json);
        }

        //pendiente deserialize config json file
        

        string rutaBase = "D:\\Usuarios\\Q27001009\\Documents\\Proyectos RPA\\RPA Calificadoras\\rutaPrueba";

        foreach(var carpeta in listaCarpetas){        

            Console.WriteLine($"creando carpeta: {carpeta.nombre} de area: {carpeta.area} en ruta: {rutaBase}");

            Directory.CreateDirectory(Path.Combine(rutaBase, carpeta.nombre));

        }

    }

    public class Carpeta{
        public string nombre { get; set; }
        public string area {get; set; }

    }

    public class Config {
        
        [JsonPropertyName("rutaBase")]
        public string rutaBase { get; set; }
    }
}
