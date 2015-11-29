using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;

namespace Task2
{
    [TestClass]
    public class OnlinerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);
            driver.Navigate().GoToUrl("http://onliner.by/");

            IWebElement enterElem = driver.FindElement(By.XPath(@"//*[@id=""userbar""]/div[2]/div[1]"));
            enterElem.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.CssSelector(@"#auth-container__forms > div > div.auth-box__field > form > div:nth-child(1) > div:nth-child(1) > input")));
            
            IWebElement emailElem = driver.FindElement(By.CssSelector(@"#auth-container__forms > div > div.auth-box__field > form > div:nth-child(1) > div:nth-child(1) > input"));
            IWebElement passwordElem = driver.FindElement(By.CssSelector(@"#auth-container__forms > div > div.auth-box__field > form > div:nth-child(1) > div:nth-child(2) > input"));
            emailElem.SendKeys(@"9nadeya15@mail.ru");
            passwordElem.SendKeys(@"nadeya");
            IWebElement submitElem = driver.FindElement(By.XPath(@"//*[@id=""auth-container__forms""]/div/div[2]/form/div[4]/div/button"));
            submitElem.Click();

            wait.Until(d => d.FindElement(By.XPath(@"//*[@id=""userbar""]/div[1]/p/a")));

            ReadOnlyCollection<IWebElement> catalog = driver.FindElements(By.ClassName("catalog-bar__item"));
            Random random = new Random();
            int catalogItemNumber = random.Next(0, catalog.Count - 1);
            IEnumerator catalogEnum = catalog.GetEnumerator();
            for (int i = 0; i <= catalogItemNumber; i++)
            {
                catalogEnum.MoveNext();
            }
            IWebElement catalogItem = (IWebElement)catalogEnum.Current;
            String catalogItemText = catalogItem.Text;
            catalogItem.Click();

            wait.Until(d => d.FindElement(By.ClassName("schema-header__title")));

            IWebElement headerTitle = driver.FindElement(By.ClassName("schema-header__title"));
            Assert.AreEqual(catalogItemText, headerTitle.Text);

            IWebElement exitElem = driver.FindElement(By.XPath(@"//*[@id=""userbar""]/div[1]/a"));
            exitElem.Click();

            driver.Quit();
        }
    }
}
