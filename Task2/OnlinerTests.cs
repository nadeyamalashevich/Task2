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

        private void CatalogTest(IWebDriver driver, Logger logger)
        {
            logger.Log("Open browser");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(100));
            driver.Navigate().GoToUrl("http://onliner.by/");

            IWebElement enterElem = driver.FindElement(By.XPath(enterByXpath));
            logger.Log("Go to url http://onliner.by/");
            Action<IWebElement> enterAction = new Action<IWebElement>(MouseClick);
            enterAction.Invoke(enterElem);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.CssSelector(emailByCss)));
            logger.Log("Go to authorization form");

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
            logger.Log("Authorization and go to main page");

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
            logger.Log("Go to catalog item");

            IWebElement headerTitle = driver.FindElement(By.ClassName(headerByClass));
            Assert.AreEqual(catalogItemText, headerTitle.Text);
            if (catalogItemText.Equals(headerTitle.Text))
                logger.Log("Display correct catalog item");

            IWebElement exitElem = driver.FindElement(By.XPath(exitByXpath));
            enterAction.Invoke(exitElem);
            logger.Log("log out");
        }

        [TestMethod]
        public void CatalogTestChrome()
        {
            Logger logger = new Logger("CatalogTestLoggerForChrome.txt");
            chromeDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);
            logger.Log("Start Chrome testing");
            CatalogTest(chromeDriver, logger);
            logger.Log("Close browser");
            logger.Dispose();
            if (chromeDriver != null)
                chromeDriver.Quit();
        }

        [TestMethod]
        public void CatalogTestFirefox()
        {
            Logger logger = new Logger("CatalogTestLoggerForFirefox.txt");
            firefoxDriver = new FirefoxDriver();
            logger.Log("Start Firefox testing");
            CatalogTest(firefoxDriver, logger);
            logger.Log("Close browser");
            logger.Dispose();
            if (firefoxDriver != null)
                firefoxDriver.Quit();
        }

        [TestMethod]
        public void CatalogTestExplorer()
        {
            Logger logger = new Logger("CatalogTestLoggerForExplorer.txt");
            explorerDriver = new InternetExplorerDriver(AppDomain.CurrentDomain.BaseDirectory, new InternetExplorerOptions(), new TimeSpan(0, 10, 0));
            logger.Log("Start Explorer testing");
            CatalogTest(explorerDriver, logger);
            logger.Log("Close browser");
            logger.Dispose();
            if (explorerDriver != null)
                explorerDriver.Quit();
        }
    }
}
