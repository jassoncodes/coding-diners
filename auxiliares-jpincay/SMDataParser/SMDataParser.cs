using AppConfig = SMDataParser.Config.AppConfig;
using Estandar = SMDataParser.Models.Estandar;
using FileManager = SMDataParser.Models.FileManager;
using DataManipulator = SMDataParser.Models.DataManipulator;
using Log = Serilog.Log;

namespace SMDataParser
{
    internal class SMDataParser
    {
        static void Main(string[] args)
        {
            
            AppConfig appConfig = new();
            DataManipulator dataManipulator = new();
            Estandar dataEstandar = new();
            FileManager fileManager = new();

            try
            {                    

                if (args.Length == 5)
                {
                    appConfig.inputPath = args[0];
                    appConfig.outputPath = args[1];
                    appConfig.logPath = args[2];
                    appConfig.inputFileName = args[3];
                    appConfig.outputFileName = args[4];
                }
                
                appConfig.configureLog();

                List<string> dataList = dataManipulator.GetData(appConfig.inputPath);
                List<Estandar> dataToWrite = dataManipulator.ParseData(dataList);
                fileManager.WriteFile(dataToWrite);
                
                Log.Information($"\t******* PROCESO TERMINADO CON ÉXITO ******* ");
                Log.Information($"\t******* Registros escritos: {dataToWrite.Count}");
                fileManager.DeleteInput(string.Concat(appConfig.inputPath,appConfig.inputFileName));

            }
            catch(Exception e)
            {
                Log.Error($"SMDataParser Error: {e.ToString()}");
            }
        }
    }
}