using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using OpenQA.Selenium.Interactions;
using System.Security.Cryptography;
using OpenQA.Selenium.Html5;

namespace Selenium_Demo
{
    class Selenium_Demo
    {
        String test_url = "https://www.booking.com/";

        IWebDriver driver;

        [SetUp]
        public void start_Browser()
        {
            // Local Selenium WebDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = test_url;
            System.Threading.Thread.Sleep(2000);
        }


        public void Close_Cookies()
        {
            driver.FindElement(By.XPath("//button[@data-gdpr-consent='accept']")).Click();
            System.Threading.Thread.Sleep(2000);
        }
        public void Search(String Location)
        {
            IWebElement Search_bar = driver.FindElement(By.CssSelector("input#ss"));
            Search_bar.SendKeys(Location);
            System.Threading.Thread.Sleep(2000);
        }

        public void Date(int months)
        {
            driver.FindElement(By.CssSelector("div.xp__dates-inner")).Click();
            System.Threading.Thread.Sleep(2000);
            IWebElement Date_Next = driver.FindElement(By.XPath("//div[@class='bui-calendar__control bui-calendar__control--next']"));
            System.Threading.Thread.Sleep(1000);
            Date_Next.Click();
            System.Threading.Thread.Sleep(1000);
            Date_Next.Click();
            System.Threading.Thread.Sleep(2000);
            DateTime today = DateTime.Now;
            DateTime futureDate = today.AddMonths(months);
            String date_2 = futureDate.ToString();
            String date_day = date_2.Substring(0, 2);
            String date_month = date_2.Substring(3, 2);
            String date_year = date_2.Substring(6, 4);
            String date_combined = date_year + "-" + date_month + "-" + date_day;
            driver.FindElement(By.XPath($"//td[@data-date='{date_combined}']")).Click();
            System.Threading.Thread.Sleep(2000);
            IWebElement search = driver.FindElement(By.CssSelector("button.sb-searchbox__button "));
            search.Click();
        }

        public bool Error_exists()
        {
            try
            {
                IWebElement Error = driver.FindElement(By.CssSelector("div.sb-searchbox__error -visible"));
                return true;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Error not shown");
                return false;
            }
        }


        [Test]
        public void test_Sauna()
            //Cant get the Sauna Tag to appear on the website but this works when it does appear
        {
            Close_Cookies();
            Search("Limerick");
            Date(3);
            System.Threading.Thread.Sleep(8000);
            try
            {
                IWebElement sauna = driver.FindElement(By.XPath("//a[@data-value = '10']"));
                Actions actions = new Actions(driver);
                actions.MoveToElement(sauna).Click();
                actions = (Actions)actions.Build();
                actions.Perform();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Sauna filter not present");
            }
            System.Threading.Thread.Sleep(4000);
            // int strand = driver.FindElements(By.XPath("//span[text() = 'Limerick Strand Hotel']")).Count; // Cant get this to work
            int strand = driver.FindElements(By.CssSelector("div#hotel_40345")).Count;
            Console.WriteLine(strand);
            Assert.True(strand == 1);
            int george = driver.FindElements(By.CssSelector("div#hotel_40243")).Count;
            Assert.True(george == 0);
        }
        [Test]
        public void test_Stars()
        {
            Close_Cookies();
            Search("Limerick");
            Date(3);
            System.Threading.Thread.Sleep(8000);
            try
            {
                IWebElement Stars_5 = driver.FindElement(By.XPath("//a[@data-id = 'class-5']"));
                Actions actions = new Actions(driver);
                actions.MoveToElement(Stars_5).Click();
                actions = (Actions)actions.Build();
                actions.Perform();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Sauna filter not present");
            }
            System.Threading.Thread.Sleep(4000);
            int savoy = driver.FindElements(By.CssSelector("div#hotel_260219")).Count;
            Assert.True(savoy == 1);
            int george = driver.FindElements(By.CssSelector("div#hotel_40243")).Count;
            Assert.True(george == 0);
        }
        [Test]
        public void Stress_Test()
        {
            System.Threading.Thread.Sleep(2000); // should really be using WaitUntilElementLoaded but couldnt find a C# equivalent
            String Long_String = new string('*', 200);
            Search(Long_String);
            System.Threading.Thread.Sleep(2000);
            Error_exists();
            Search(" ");
            System.Threading.Thread.Sleep(2000);
            Error_exists();
        }

        [TearDown]
        public void close_Browser()
        {
            driver.Quit();
        }
    }
}