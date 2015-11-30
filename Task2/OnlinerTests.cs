using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Configuration;

namespace Task2
{
    [TestClass]
    public class OnlinerTests
    {
        private IWebDriver chromeDriver = null;
        private IWebDriver firefoxDriver = null;
        private IWebDriver explorerDriver = null;

        private void CatalogTest(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://onliner.by/");

            String enterByXpath = ConfigurationManager.AppSettings["enterByXpath"];
            IWebElement enterElem = driver.FindElement(By.XPath(enterByXpath));
            enterElem.Click();
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            String emailByCss = ConfigurationManager.AppSettings["emailByCss"];
            wait.Until(d => d.FindElement(By.CssSelector(emailByCss)));
            
            IWebElement emailElem = driver.FindElement(By.CssSelector(emailByCss));
            String passwordByCss = ConfigurationManager.AppSettings["passwordByCss"];
            IWebElement passwordElem = driver.FindElement(By.CssSelector(passwordByCss));
            emailElem.SendKeys(@"9nadeya15@mail.ru");
            passwordElem.SendKeys(@"nadeya");

            String submitByXpath = ConfigurationManager.AppSettings["submitByXpath"];
            IWebElement submitElem = driver.FindElement(By.XPath(submitByXpath));
            submitElem.Click();

            String userbarByXpath = ConfigurationManager.AppSettings["userbarByXpath"];
            wait.Until(d => d.FindElement(By.XPath(userbarByXpath)));
            String catalogbarByClass = ConfigurationManager.AppSettings["catalogbarByClass"];
            wait.Until(d => d.FindElement(By.ClassName(catalogbarByClass)));

            ReadOnlyCollection<IWebElement> catalog = driver.FindElements(By.ClassName(catalogbarByClass));
            Random random = new Random();
            int catalogItemNumber = random.Next(0, catalog.Count - 1);
            IEnumerator catalogEnum = catalog.GetEnumerator();
            for (int i = 0; i <= catalogItemNumber; i++)
            {
                catalogEnum.MoveNext();
            }
            IWebElement catalogItem = (IWebElement)catalogEnum.Current;
            wait.Until(d => catalogItem.Displayed);
            String catalogItemText = String.Copy(catalogItem.Text);
            catalogItem.Click();

            String headerByClass = ConfigurationManager.AppSettings["headerByClass"];
            wait.Until(d => d.FindElement(By.ClassName(headerByClass)));

            IWebElement headerTitle = driver.FindElement(By.ClassName(headerByClass));
            Assert.AreEqual(catalogItemText, headerTitle.Text);

            String exitByXpath = ConfigurationManager.AppSettings["exitByXpath"];
            IWebElement exitElem = driver.FindElement(By.XPath(exitByXpath));
            exitElem.Click();
        }

        [TestInitialize]
        public void DriverInitializer()
        {
            chromeDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);
            //explorerDriver = new InternetExplorerDriver(AppDomain.CurrentDomain.BaseDirectory, new InternetExplorerOptions(), new TimeSpan(0, 10, 0));
            //firefoxDriver = new FirefoxDriver();
        }

        [TestMethod]
        public void CatalogTestChrome()
        {
            CatalogTest(chromeDriver);
        }

        //[TestMethod]
        //public void CatalogTestFirefox()
        //{
        //    CatalogTest(firefoxDriver);
        //}

        //[TestMethod]
        //public void CatalogTestExplorer()
        //{
        //    CatalogTest(explorerDriver);
        //}

        [TestCleanup]
        public void DisposeDriver()
        {
            try
            {
                if (chromeDriver != null)
                    chromeDriver.Quit();
                if (firefoxDriver != null)
                    firefoxDriver.Quit();
                if (explorerDriver != null)
                    explorerDriver.Quit();
            }
            catch
            {
            }
        }
    }
}
