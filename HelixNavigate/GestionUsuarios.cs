using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace helixIntegration
{
    class GestionUsuarios : HelperRpa
    {
        private IWebDriver driverInterface;

        public GestionUsuarios(IWebDriver driver) : base(driver) => driverInterface = driver;

        internal bool gestionUsuario(IWebDriver driver, bool isAccess)
        {
            if (isAccess)
            {
                Console.WriteLine("Abrir el catalogo");
                var tramiteUrl = @"https://dceservice-dwp.onbmc.com/dwp/app/#/catalog/section/332;type=SBE;providerSourceName=SBE";
                driver.Navigate().GoToUrl(tramiteUrl);

                try
                {
                    Thread.Sleep(5000);
                    var pageCreateService = @"//main[@id='start']/dwp-immersive[1]/div[1]/div[1]/div[2]/section[1]/div[1]/div[1]/dwp-tombstone-card[1]/dwp-regular-card[1]/div[1]/dwp-icon-media[1]/div[1]/div[1]/div[2]/dwp-cost[1]/span[1]";
                    driver.FindElement(By.XPath(pageCreateService)).Click();
                    Thread.Sleep(2000);
                    return true;

                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message); 
                    return false;
                }


            }
            return false;
        }
    }
}
