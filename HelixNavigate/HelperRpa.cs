using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace helixIntegration
{
    internal class HelperRpa
    {
        private IWebDriver driverInterface;

        public HelperRpa(IWebDriver driver) => driverInterface = driver;

        internal void findButtonClick(string field, int timeInit, int timefisnish)
        {
            var enviarPeticionButton = driverInterface.FindElement(By.XPath(field));
            Thread.Sleep(timeInit);
            enviarPeticionButton.Click();
            Thread.Sleep(timefisnish);
        }
        internal void findFieldClick(string field)
        {
            var fieldSearch = field;
            var optionOperation = driverInterface.FindElement(By.XPath(fieldSearch));
            optionOperation.Click();
        }
        internal void findFieldSetText(string field, string valueField)
        {
            var fieldSearch = field;
            var optionOperation = driverInterface.FindElement(By.XPath(fieldSearch));
            optionOperation.SendKeys(valueField);
        }
        internal string cleanString(string imputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char charterInput in imputString.Trim())
            {
                if ((charterInput >= '0' && charterInput <= '9') || (charterInput >= 'A' && charterInput <= 'Z') || (charterInput >= 'a' && charterInput <= 'z') || charterInput == '.' || charterInput == '_')
                {
                    sb.Append(charterInput);
                }
            }
            return sb.ToString();
        }

        public static string ValidateInputFilePath(String path)
        {
            /* 
             * valida ruta, obtiene archivo mas reciente y retorna  ruta completa
             */

            string recentFilePath = "";

            try
            {
                //Lee directorio en busqueda de archivo mas reciente
                var directory = new DirectoryInfo(path);
                recentFilePath = path;
                return recentFilePath;

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
