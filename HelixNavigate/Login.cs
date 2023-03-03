<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
=======
﻿
using OpenQA.Selenium;

>>>>>>> origin/developer

namespace helixIntegration
{
     class Login
    {
   
        internal bool access(IWebDriver driver)
        {
            String catalogoUrl = "https://dceservice-dwp.onbmc.com/dwp/app/#/catalog";
            String user = "usrbotrunner";
            String password = "BotInterdin.2002";

            driver.Navigate().GoToUrl(catalogoUrl);
            Thread.Sleep(3000);
            try
            {
                string loginUrl = "https://or-rsso1.onbmc.com/rsso/start";
                string currectUrl = driver.Url;
                Console.WriteLine(currectUrl);
                if (currectUrl == loginUrl)
                {
                    IWebElement usuarioInput = driver.FindElement(By.Id("user_login"));
                    usuarioInput.SendKeys(user);

                    IWebElement passwordInput = driver.FindElement(By.Id("login_user_password"));
                    passwordInput.SendKeys(password);

                    IWebElement bottonLogin = driver.FindElement(By.Id("login-jsp-btn"));
                    bottonLogin.Click();
                    Thread.Sleep(3000);

                    return true;

                }
            }catch(WebDriverException error) {

                Console.WriteLine(error.ToString());
                return false;
            }
            return false;

        }
    }
}
