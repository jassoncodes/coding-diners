
namespace BaseGeneraticaSpliter
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string rutaBaseGeneratica = @"E:\RECURSOS ROBOT\DATA\BUSQUEDAJUICIOS\ARCHIVOS\BaseGeneratica\Resultado Generatica para historial BJ.xlsx";

            string rutaBasePorCorte = @"E:\RECURSOS ROBOT\DATA\BUSQUEDAJUICIOS\ARCHIVOS\BasesPorCorte";

            string logPath = @"E:\RECURSOS ROBOT\LOGS\BUSQUEDAJUICIOS\";

            ExcelSplitter.SplitExcelFile(rutaBaseGeneratica, 10, rutaBasePorCorte);

        }

    }
}