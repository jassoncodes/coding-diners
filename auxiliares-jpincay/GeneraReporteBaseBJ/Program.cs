namespace GeneraReporteBaseBJ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceFile = @"E:\RECURSOS ROBOT\DATA\BUSQUEDAJUICIOS\ARCHIVOS\BaseGeneratica\Resultado Generatica para historial BJ.xlsx";
            string outputFile = @"E:\RECURSOS ROBOT\DATA\BUSQUEDAJUICIOS\ARCHIVOS\ReporteFinal\REPORTE-RPA-BJ.xlsx";
            string fileName = "";
            ExcelDuplicateFilter.FilterDuplicates(sourceFile, outputFile);
        }
    }
}