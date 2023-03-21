using Serilog;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Microsoft.Office.Interop.Excel;
using RPAHelper = ActualizarReqSM.NavigatorSM.HelperRpa;
using OpenQA.Selenium.Chrome;
using ActualizarReqSM.Models;
using DriverWeb = ActualizarReqSM.NavigatorSM.DriverWeb;

namespace ActualizarReqSM.NavigatorSM
{
    class WebSM {

        public string urlSM;
        public string userSM;
        public string passSM;

        private string loggedInURL;
        private string mainPanel;
        private string inputBuscarPeticion;
        private double defaultWaitTime;

        private ChromeDriver driver;

        private RPAHelper helperRpa;

        public WebSM() { 

            urlSM = string.Empty;
            userSM = string.Empty;
            passSM = string.Empty;

            loggedInURL = "https://smgestion.uio.bpichincha.com/sm960/index.do";
            mainPanel = "//*[@id='ext-gen-top53']/em/span/span";
            //inputBuscarPeticion = "//*[@id=\"X21\"]";
            inputBuscarPeticion = "//*[@id=\"X21\"]";
            defaultWaitTime = 10;

            driver = new DriverWeb().GetChromeDriver();
            helperRpa = new(driver);

        }

        public void AbrirPanelBusquedaPeticion()
        {
            try
            {
                //Thread.Sleep(1000);

                Log.Information($"Ingresando a Panel Buscar peticiones...");

                //opcion cumplimiento peticiones
                helperRpa.ClickWaitField("//*[@id=\"ROOT/Cumplimiento de peticiones\"]", defaultWaitTime);
                //helperRpa.ClickWaitField("/[@id=\"ext-gen-top240\"]", defaultWaitTime);
                //helperRpa.findFieldClickWait("//*[@id=\"ROOT/Cumplimiento de peticiones\"]", 5);

                //Thread.Sleep(1000);
                //opcion buscar peticiones
                helperRpa.ClickWaitField("//*[@id=\"ROOT/Cumplimiento de peticiones/Buscar peticiones\"]", defaultWaitTime);
                //helperRpa.ClickWaitField("//*[@id=\"ext-gen-top243\"]/span", defaultWaitTime);
                //helperRpa.findFieldClickWait("//*[@id=\"ROOT/Cumplimiento de peticiones/Buscar peticiones\"]", 5);
                //Thread.Sleep(1000);



                List<IWebElement> iFrames = driver.FindElements(By.TagName("iframe")).ToList<IWebElement>();

                Log.Warning($"iFrames: {iFrames.Count}");

                IWebElement iFrame = iFrames.Last();

                driver.SwitchTo().Frame(iFrames.Last());

                Log.Warning($"Switched to frame: {iFrames.IndexOf(iFrames.Last())}");


            }
            catch (Exception e)
            {
                Log.Error($"AbrirPanelBusquedaPeticion() Error: No se pudo abrir panel de busqueda de peticiones\n{e}");
            }
        }

        public bool ElementDisplayed(string xPathElement)
        {
            bool displayed = false;

            try
            {
                //Thread.Sleep(5000);

                displayed = driver.FindElement(By.XPath(xPathElement)).Displayed;
                //Thread.Sleep(5000);

                return displayed;

            }
            catch
            {
                return false;
            }
        }

        public void BuscarPeticion(HelixTicket ticket)
        {
            try
            {
                //Thread.Sleep(5000);

                //espera input para buscar peticion
                //if (ElementDisplayed(inputBuscarPeticion))
                //{
                    Log.Information($"Buscando peticion {ticket.idOdt}...");
                   

                    helperRpa.ClickWaitField(inputBuscarPeticion, defaultWaitTime);

                    Log.Information($"click en: {inputBuscarPeticion} ...");

                    //ingres idodt a buscar
                    helperRpa.FindFieldClearSetText(inputBuscarPeticion, ticket.idOdt);

                    Thread.Sleep(2000);

                    //clic boton buscar o send keys enter
                    var input = driver.FindElement(By.XPath(inputBuscarPeticion));
                    
                    Thread.Sleep(2000);

                    input.SendKeys(Keys.Control + Keys.Shift + Keys.F6);

                    Thread.Sleep(2000);

                //}
                //else
                //{
                //    Log.Error($"BuscarPeticion() Error: Elemento inputBuscarPeticion no se encontro..");
                //}

            }
            catch (Exception e)
            {
                Log.Error($"BuscarPeticion() Error: No se pudo buscar peticion\n{e}");
            }
        }

        public void ActualizarPeticion(HelixTicket ticket)
        {

            try
            {

                //espera tab actividades
                if (ElementDisplayed("//*[@id=\"X104_t\"]"))
                {
                    Log.Information($"Ingresando a panel actividades...");
                    
                    //Thread.Sleep(2000);
                    // clic tab actividades
                    helperRpa.ClickWaitField("//*[@id=\"X104_t\"]",defaultWaitTime);

                    //Thread.Sleep(2000);

                    helperRpa.ClickWaitField("//*[@id=\"X108\"]", defaultWaitTime);
                    //registra palabra actualizar
                    helperRpa.findFieldSetText("//*[@id=\"X108\"]", "Actualizar");

                    Thread.Sleep(2000);

                    string actualizacion = "Actualizacion RPA " + ticket.noReq;

                    Log.Information($"Actualizando peticion: {actualizacion}");

                    // clic text area actualizacion
                    helperRpa.findFieldClick("//*[@id=\"X112View\"]");

                    Thread.Sleep(1000);
                    // registra actualizacion
                    helperRpa.findFieldSetText("//*[@id=\"X112\"]", actualizacion);

                    Thread.Sleep(2000);

                    Log.Information($"Guardando actualizacion...");

                    //guardar actualizacion
                    var input = driver.FindElement(By.XPath("//*[@id=\"X112\"]"));
                    input.SendKeys(Keys.Control + Keys.Shift + Keys.F2);

                    Thread.Sleep(2000);

                }
                else if (ElementDisplayed("//*[@id=\"X108_t\"]"))
                {
                    Log.Warning($"Peticion no disponible para actualizar (Estado: Revisar)...");

                    Thread.Sleep(2000);

                    Log.Warning($"Siguiente peticion...");

                    var frameAct = driver.FindElement(By.TagName("body"));

                    //cierra panel actualizacion

                    frameAct.SendKeys(Keys.Alt + Keys.F3);

                }
                else
                {
                    Thread.Sleep(2000);
                    
                    var frameAct = driver.FindElement(By.TagName("body"));

                    Thread.Sleep(2000);
                    //cierra panel actualizacion
                    frameAct.SendKeys(Keys.Alt + Keys.F3);

                    Thread.Sleep(2000);

                    throw new Exception($"ActualizarPeticion() Error: No se pudo actualizar la peticion...");
                }


                Thread.Sleep(2000);

            }catch (Exception e)
            {
                Log.Error($"ActualizarPeticion() Error: Error en la actualizacion de la peticion {ticket.idOdt}\n{e}");
            }

        }

        public void LogOutSM()
        {
            try {

                //var frameAct = driver.FindElement(By.TagName("body"));

                //cierra panel actualizacion
                //frameAct.SendKeys(Keys.Alt + Keys.F3);

                Log.Information($"Cerrando sesion...");

                //Thread.Sleep(5000);

                driver.SwitchTo().Frame(0);

                helperRpa.findFieldClick("//*[@id=\"ext-gen-top104\"]");

                // //*[@id=\"ext-gen-top374\"]
                helperRpa.findFieldClick("//button[contains(text(),\"Desconexión\")]");

                IAlert iAlert = driver.SwitchTo().Alert();

                iAlert.Accept();

                Thread.Sleep(5000);

                driver.Dispose();

            } catch(Exception e) 
            {
                Log.Error($"LogOutSM() Error: No se pudo cerra la sesion\n{e}");
            }
        
        }

        public bool LogInSM(string urlSM, string userSM, string passSM)
        {
            bool loggedIn = false;

            try {

                driver.Navigate().GoToUrl(urlSM);

                string sessionId = driver.SessionId.ToString();
                Log.Information($"sessionId: {sessionId}");

                Thread.Sleep(5000);

                helperRpa.findFieldSetText("//*[@id=\"LoginUsername\"]", userSM);
                
                Thread.Sleep(500);

                helperRpa.findFieldSetText("//*[@id=\"LoginPassword\"]", passSM);
                Thread.Sleep(500);

                helperRpa.findFieldClick("//*[@id=\"loginBtn\"]");

                Thread.Sleep(5000);

                if (driver.Url == loggedInURL)
                {
                    loggedIn = true;
                }

                return loggedIn;

            } catch (Exception e)
            {
                Log.Error($"{e}");
                return loggedIn;
            }


        }


    }





}
