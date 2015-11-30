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

        private readonly String enterByXpath = ConfigurationManager.AppSettings["enterByXpath"];
        private readonly String emailByCss = ConfigurationManager.AppSettings["emailByCss"];
        private readonly String passwordByCss = ConfigurationManager.AppSettings["passwordByCss"];
        private readonly String submitByXpath = ConfigurationManager.AppSettings["submitByXpath"];
        private readonly String userbarByXpath = ConfigurationManager.AppSettings["userbarByXpath"];
        private readonly String catalogbarByClass = ConfigurationManager.AppSettings["catalogbarByClass"];
        private readonly String headerByClass = ConfigurationManager.AppSettings["headerByClass"];
        private readonly String exitByXpath = ConfigurationManager.AppSettings["exitByXpath"];
        private readonly String email = @"9nadeya15@mail.ru";
        private readonly String password = @"nadeya";

        private void MouseClick(IWebElement button)
        {
            if (button != null)
            {
                button.Click();
            }
        }

        private void CatalogTest(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://onliner.by/");

            IWebElement enterElem = driver.FindElement(By.XPath(enterByXpath));
            Action<IWebElement> enterAction = new Action<IWebElement>(MouseClick);
            enterAction.Invoke(enterElem);
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.CssSelector(emailByCss)));
            
            IWebElement emailElem = driver.FindElement(By.CssSelector(emailByCss));
            IWebElement passwordElem = driver.FindElement(By.CssSelector(passwordByCss));
            emailElem.SendKeys(email);
            passwordElem.SendKeys(password);

            IWebElement submitElem = driver.FindElement(By.XPath(submitByXpath));
            String submitTypeAttribute = submitElem.GetAttribute("type");
            if (submitTypeAttribute.Equals("submit"))
            {
                enterAction.Invoke(submitElem);
            }

            wait.Until(d => d.FindElement(By.XPath(userbarByXpath)));
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
            enterAction.Invoke(catalogItem);

            wait.Until(d => d.FindElement(By.ClassName(headerByClass)));

            IWebElement headerTitle = driver.FindElement(By.ClassName(headerByClass));
            Assert.AreEqual(catalogItemText, headerTitle.Text);

            IWebElement exitElem = driver.FindElement(By.XPath(exitByXpath));
            enterAction.Invoke(exitElem);
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
