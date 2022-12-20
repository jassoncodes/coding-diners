﻿using AppConfig = SMDataParser.Config.AppConfig;
using Estandar = SMDataParser.Models.Estandar;
using FileManager = SMDataParser.Models.FileManager;
using DataManipulator = SMDataParser.Models.DataManipulator;
using ProccessHandler = SMDataParser.Models.ProccessHandler;
using Log = Serilog.Log;

namespace SMDataParser
{
    internal class SMDataParser
    {


        static void Main(string[] args)
        {

            AppConfig appConfig = new AppConfig();
            DataManipulator dataManipulator = new DataManipulator();
            Estandar dataEstandar = new Estandar();
            FileManager fileManager = new FileManager();

            try
            {
                appConfig.configureLog();

                List<string> dataList = dataManipulator.GetData(appConfig.inputPath);
                List<Estandar> dataToWrite = dataManipulator.ParseData(dataList);
                fileManager.WriteFile(dataToWrite);
                Log.Information($"\n ******* Registros procesados: {dataToWrite.Count}");

            }
            catch(Exception e)
            {
                Log.Error($"Error: \n {e.ToString()}");
            }
        }
    }
}