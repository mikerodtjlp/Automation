/*
 * Template 
 * UI Automation
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTesting
{
    [TestClass]
    public partial class UI
    {
        bool success = false;
        bool failed = false;
        bool err_crash = false;
        bool err_not_closed = false;
        bool SoftHardChecked = false;

        /*
         * Setup default values and initialization process
         */
        [TestInitialize]
        public void TestInitialize()
        {
            Setup.TestInitialize();
        }

        /*
         * Clean up process
         */
        [TestCleanup]
        public void TestCleanup()
        {
            log("");
            log(success ? "Test Passed" : "Test Failed");
        }

        /*
         * Show the initial configuration to the Log
         */
        public void Log_Configuration()
        {
            Setup.Log_Configuration(UI.log);
        }

        /*
         * Create Specific Browser
         */
        public ChromiumDriver GetDriver(string browser, bool headless, bool startmax)
        {
            if (browser == "Chrome")
            {
                var options = new ChromeOptions();
                if (Setup.HEADLESS) options.AddArguments("headless");
                if (Setup.START_MAXIMIZED) options.AddArguments("start-maximized");
                else options.AddArguments("window-size=1920,1080");
                return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
            }
            else
            {
                var options = new EdgeOptions();
                if (Setup.HEADLESS) options.AddArgument("headless");
                if (Setup.START_MAXIMIZED) options.AddArguments("start-maximized");
                else options.AddArguments("window-size=1920,1080");
                return new EdgeDriver(options);
            }
        }

        /*
         * Chrome Browser UI testing
         */
        public void UI_Chrome()
        {
            if (Setup.CHROME) UI_Entry("Chrome");
        }
        /*
         * Edge Browser UI testing
         */
        public void UI_Edge()
        {
            if (Setup.EDGE) UI_Entry("Edge");
        }

        /*
         * UI testing Entry
         */
        public void UI_Entry(string browser)
        {
            if (Setup.CURRENT_STAGE == "") return;

            Log_Configuration();

            if (Setup.URL == "" && Setup.ENV != "" && Setup.SITE != "")
                Setup.URL = UIData.URLS[Setup.ENV][Setup.SITE];

            using (var driver = GetDriver(browser, Setup.HEADLESS, Setup.START_MAXIMIZED))
            {
                log("Driver " + browser + " created");
                steps(driver);
                driver.Close();
                driver.Quit();
            }
        }
        /*
         *  Steps to test according witthe configuartion
         */
        public void steps(ChromiumDriver driver)
        {
            try
            {
                if (Setup.CONNECT)
                {
                    if (Setup.LOGIN)
                    {
                        Connect(driver, Setup.URL);
                        WaitSiteLoaded(driver);
                        /*if (!Log_In(driver, Setup.USER))
                        {
                            driver.Navigate().Refresh();
                            log("Refresh");
                            WaitSiteLoaded(driver);
                            Log_In(driver, Setup.USER);
                        }*/
                        log("");
                    }
                    else
                    {
                        Connect(driver, Setup.URL + "/Direct");
                        WaitSiteLoaded(driver);
                    }
                }
                //if (Setup.CHECKOPER) UI_MES(driver);
                //if (Setup.CHECKUNIT) UI_Lang(driver);
                //if (Setup.CHECKUNIT) UI_Unit(driver);
                //if (Setup.LOGIN) UI_Exit(driver);

                success = true;
            }
            catch (Exception ex)
            {
                log(""); log("");
                log("Error: " + ex.Message);
                log("Cause: " + ex.InnerException);
                log("Stack: " + ex.StackTrace);
                takeScreenshot(driver);
                Assert.Fail("Unhandle Exception!");
            }

            if (failed) Assert.Fail("Error MES module!");
            if (!success) Assert.Fail("Error in automation!");
        }

        /*
         * Conection to the target url
         */
        public void Connect(ChromiumDriver driver, string site)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Setup.TIMEOUT);
            log("Connect to: " + site);
            driver.Navigate().GoToUrl(site);
            log("Connected");
        }

        /*
         * We have to wait ont only for connect to the application but wait for it fully load the behind stuff
         */
        public void WaitSiteLoaded(ChromiumDriver driver)
        {
            for (int i = 0; i < 2; i++)
            {
                //WaitPageLoaded(driver, 4 * 8192);
                //WaitPageWorkable(driver, 4 * 8192);
                //WaitForJS(driver, "document.getElementById('linesDropdown').options.length > 1 ? 'DONE' : ''", "DONE", 8192);
            }
        }

        /*
         * Unit Page testing process
         */
        public void UI_Unit(ChromiumDriver driver)
        {
            WaitPageLoaded(driver, 8192);
            WaitLangLoaded(driver, 8192);

            log("");
            for (int i = 0; i < 2; ++i)
            {
                log("Clicking Unit");
                WaitExistElement(driver, By.Id("unitLink"), 8192);
                GetClickableElement(driver, By.Id("unitLink"), 8192).Click();
                WaitPageLoaded(driver, 8192);

                for (int j = 0; j < 4; ++j)
                {
                    // Get links
                    WaitExistElement(driver, By.XPath("//div[@class='ml-48']/ul"), 8192);
                    var links = driver.FindElement(By.XPath("//div[@class='ml-48']/ul"));
                    var link = links.FindElements(By.TagName("li"));
                    var anch = link[i].FindElement(By.TagName("a"));
                    var href = anch.GetAttribute("href");
                    var hok = href != null && href != "";
                    if (!hok)
                    {
                        Thread.Sleep(1024);
                        continue;
                    }

                    log("hrefs : " + link.Count.ToString());
                    log("Clicking : " + anch.Text);
                    log("href: " + href);
                    anch.Click();

                    WaitPageLoaded(driver, 8192);
                    Thread.Sleep(1024);
                    WaitLangLoaded(driver, 8192);

                    if (hok) break;
                }
            }
        }
        /*
         * langiage rtesting
         */
        public bool UI_Lang(ChromiumDriver driver)
        {
            log("Checking Language");
            /*
            // select line
            var comboloaded = false;
            WaitExistElement(driver, By.Id("languageButton"), 8192);
            var query = GetElement(driver, By.Id("languageButton"), 8192);
            for (int i = 0; i < 16 * 1000; i += 250)
            {
                new Actions(driver).MoveToElement(query);
                comboloaded = (driver as IJavaScriptExecutor).
                    ExecuteScript("return document.getElementById('linesDropdown').disabled ? '' : 'FREE'").Equals("FREE");
                if (comboloaded) break;
                Thread.Sleep(250);
            }
            if (!comboloaded) return false;
            log("Dropdown clicked");

            lg("Selecting Line...");
            IWebElement iWebelement = driver.FindElement(By.Id("linesDropdown"));       // Getting the element of the Text Box
            IWebElement iWebelementList = driver.FindElement(By.Id("linesDropdown"));   // Getting the elements of List/Drop down Box
            SelectElement selected = new SelectElement(iWebelementList);                // Parsing the list
            iWebelement.SendKeys(Keys.ArrowDown);                                       // Clicking the drop down image
            Thread.Sleep(1024);                                                         // wait to fill combo box
            selected.SelectByIndex(1);                                                  // select index 1 cause index 0 is the same text as "select..."
            log("Selected ListBox option 1");

            WaitPageLoaded(driver, 8192);
            */
            log("Language Checked");
            return true;
        }


        /*
         * Exit from the Main page, it goes in thoery into a log in page
         */
        public void UI_Exit(ChromiumDriver driver)
        {
            lg("Closing WebApp...");
            WaitPageLoaded(driver, 8192);
            //Thread.Sleep(1024);
            WaitLangLoaded(driver, 8192);
            WaitExistElement(driver, By.ClassName("fa-power-off"), 8192);
            GetClickableElement(driver, By.ClassName("fa-power-off"), 8192).Click();
            log("Closed WebApp");
        }

        /*
         * Check if the Station's Title exists
         */
        private bool hasStationTitle(ChromiumDriver driver)
        {
            if (ExistElement(driver, By.XPath("//div[@class='p-2 stationDescription']")))
            {
                var spans = driver.FindElements(By.XPath("//div[@class='p-2 stationDescription']/h2/span"));
                return spans.Count > 0;
            }
            return false;
        }
        /*
         * Get station name
         */
        private string GetStationName(ChromiumDriver driver)
        {
            string station = "";
            if (ExistElement(driver, By.XPath("//div[@class='p-2 stationDescription']")))
            {
                var spans = driver.FindElements(By.XPath("//div[@class='p-2 stationDescription']/h2/span"));
                if (spans.Count == 0) return station;
                try
                {
                    station = spans[0].Text;
                }
                catch (Exception) { }
            }
            return station;
        }

        /*
         * Closing the station option pressing close button to return main page 
         */
        private void Close_Option(ChromiumDriver driver, string option)
        {
            IWebElement query = null;
            lg("Clicking close Button...");

            var patch = option == "(ALIGNMENT/CURING)Button" || option == "(READY TO COLLECT)Button" ||
                        option == "(WRITE OFF)Button" || option == "Station Overview" ||
                        option == "FLOOR MAP" || option == "USER OVERVIEW" ||
                        option == "KPI BOARD" || option == "Defects" || option == "WIP Order";

            var CPcls = option == "TRACKING" || option == "CAPACITY PLANNING";
            var strms = option == "Stream Settings";
            By by = null;

            if (!patch && !CPcls)
            {
                // mostly GAT
                if (WaitExistElement(driver, By.Id("closeButton"), 2048))
                    by = By.Id("closeButton");
                else if (WaitExistElement(driver, By.Id("cancelButton"), 2048))
                    by = By.Id("cancelButton");
            }
            else if (patch && WaitExistElement(driver, By.XPath("//i[@class='fa fa-power-off']"), 2048))
            {
                by = By.XPath("//i[@class='fa fa-power-off']");
            }
            else if (CPcls && WaitExistElement(driver, By.Id("closeBtn"), 2048))
            {
                by = By.Id("closeBtn");
            }
            //else if (strms && WaitExistElement(driver, By.XPath("//*[@id='mainWindow']/div[2]/div/div/div[1]/div[1]/div[1]/i"), 2048))
            //    query = GetClickableElement(driver, By.XPath("//*[@id='mainWindow']/div[2]/div/div/div[1]/div[1]/div[1]/i"), 2048);

            if (by == null) fail("Couldnt find a close button");

            CheckForError(driver, 512);
            CheckReconnect(driver, 512);
            query = GetClickableElement(driver, by, 2048);
            new Actions(driver).MoveToElement(query);
            query.Click();
            log("Clicked");
        }

        private static void log(string msg) { if (Setup.LOG) Console.WriteLine(msg); }
        private static void lg(string msg) { if (Setup.LOG) Console.Write(msg); }
        private static void fail(string msg) { log(msg); Assert.Fail(msg); }

        /*
         * Wait for a specific page to load, trys to wait as much as possible but crashes after some time
         */
        private void WaitPageLoaded(ChromiumDriver driver, Int32 milliscnds)
        {
            WaitForJS(driver, "document.readyState", "complete", milliscnds);
        }
        /*
         * Even when the page is ready, there's more things process on background, so we wait until some good
         * signal of finish process, in this case the number registered of css, we need to find out a better criteria
         */
        private void WaitPageWorkable(ChromiumDriver driver, Int32 milliscnds)
        {
            //WaitForJS(driver, "document.styleSheets.length === 10 ? 'DONE' : ''", "DONE", milliscnds);
        }
        /*
         * it seems after new page, the language region its loaded taking time
         */
        private void WaitLangLoaded(ChromiumDriver driver, Int32 milliscnds)
        {
            for (int timeout = 0; timeout < milliscnds; timeout += 256)
            {
                if (driver.FindElements(By.Name("language")).Count > 0) break;
                Thread.Sleep(256);
            }
        }
        /*
         * Generic function for wait custom javascript code
         */
        private void WaitForJS(ChromiumDriver driver, string jsc, string cmp, Int32 milliscnds)
        {
            for (int timeout = 0; timeout < milliscnds; timeout += 256)
            {
                if ((driver as IJavaScriptExecutor).ExecuteScript("return " + jsc).Equals(cmp)) break;
                Thread.Sleep(256);
            }
        }
        /*
         * Some elements change from disable to undisable/clickable
         */
        private IWebElement GetClickableElement(ChromiumDriver driver, By by, int milliscnds)
        {
            // wait until element is created
            WaitExistElement(driver, by, milliscnds);
            // wait until element if setupd
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(milliscnds));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            // wait until element is clickable
            wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(milliscnds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
        }
        /*
         * Some elements change from disable to undisable/clickable
         */
        private IWebElement GetElement(ChromiumDriver driver, By by, int milliscnds)
        {
            // wait until element is created
            WaitExistElement(driver, by, milliscnds);
            // wait until element if setupd
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(milliscnds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
        }
        private IWebElement FindElement(ChromiumDriver driver, By by)
        {
            return driver.FindElement(by);
        }

        /*
         * We use this function when we are certain that the page is fully loaded
         */
        private bool ExistElement(ChromiumDriver driver, By by)
        {
            // identify elements and store it in list
            List<IWebElement> e = new List<IWebElement>();
            e.AddRange(driver.FindElements(by));
            return e.Count > 0; // checking element count in list
        }

        /*
         * We use this function when there's a possbility that the page is not fully loaded
         */
        private bool WaitExistElement(ChromiumDriver driver, By by, int milliscnds)
        {
            for (int timeout = 0; timeout < milliscnds; timeout += 256)
            {
                // identify elements and store it in list
                List<IWebElement> e = new List<IWebElement>();
                e.AddRange(driver.FindElements(by));
                if (e.Count > 0) return true; // checking element count in list
                Thread.Sleep(256);
            }
            return false;
        }
        /*
         * Some elements having a delay between the are created and they are visible
         */
        private bool WaitElementIsVisible(ChromiumDriver driver, By by, int milliscnds)
        {
            for (int timeout = 0; timeout < milliscnds; timeout += 256)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(16));
                var query = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
                if (query.Displayed && query.Enabled) return true;
                Thread.Sleep(256);
            }
            return false;
        }
        public IWebElement activeElement(ChromiumDriver driver)
        {
            return (IWebElement)(driver as IJavaScriptExecutor).ExecuteScript("return document.activeElement;");
        }
        public void CheckReconnect(ChromiumDriver driver, int milliscnds)
        {
            if (WaitExistElement(driver, By.Id("components-reconnect-modal"), 512))
            {
                lg("Reconnection Occurred...");
                for (int i = 0; i < 8192; i += 256)
                {
                    if (!ExistElement(driver, By.Id("components-reconnect-modal")))
                        break;
                    //if (!driver.FindElement(By.Id("components-reconnect-modal")).Displayed) 
                    //    break;
                    if (!ExistElement(driver, By.Id("components-reconnect-modal")))
                        break;
                    //if (!driver.FindElement(By.Id("components-reconnect-modal")).Enabled) 
                    //    break;
                    //if (!ExistElement(driver, By.Id("components-reconnect-modal"))) break;
                    //var query = GetElement(driver, By.Id("components-reconnect-modal"), 256);
                    //if (!query.Displayed || !query.Enabled) break;
                    Thread.Sleep(256);
                }
                log("Reconnection Finished");
            }
        }
        /*
         * takes ang set the screenshot into Testing summary report as an attachment
         */
        public void takeScreenshot(ChromiumDriver driver)
        {
            //var filePath = "TestResults\\" + (Setup.WORKORDER == null || Setup.WORKORDER == "" ? "" : Setup.WORKORDER) + "_Crash.jpg";
            Directory.CreateDirectory(TestContext.TestResultsDirectory);
            var filePath = TestContext.TestResultsDirectory + "\\Screenshot_" + TestContext.TestName + DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss") + ".png";
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);
            TestContext.AddResultFile(filePath);
        }
        public void CheckForError(ChromiumDriver driver, int milliscnds)
        {
            err_crash = false;
            if (ErrorOcurred(driver, milliscnds))
            {
                log("CRASH!!!");
                var entries = driver.Manage().Logs.GetLog(LogType.Browser);
                foreach (var entry in entries)
                {
                    var ent = entry.ToString();
                    ent = ent.Replace("\\n", Environment.NewLine);
                    log(entry.ToString().Replace("\\n", Environment.NewLine));
                }
                err_crash = true;
                takeScreenshot(driver);
                Assert.Fail("Unhandle Exception When Checking Order");
            }
        }
        public bool ErrorOcurred(ChromiumDriver driver, int milliscnds)
        {
            //if (WaitExistElement(driver, By.Id("blazor-error-ui"), milliscnds))
            //    return driver.FindElement(By.Id("blazor-error-ui")).GetCssValue("display") == "block";
            return false;
        }
        public TestContext TestContext { get; set; }
    }
}
