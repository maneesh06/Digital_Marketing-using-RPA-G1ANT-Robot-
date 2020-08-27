/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Selenium
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;


using Firefox = OpenQA.Selenium.Firefox;
using Chrome = OpenQA.Selenium.Chrome;
using IE = OpenQA.Selenium.IE;
using Edge = OpenQA.Selenium.Edge;
using Safari = OpenQA.Selenium.Safari;
using Opera = OpenQA.Selenium.Opera;

using System.IO;
using OpenQA.Selenium.Remote;
using System.Diagnostics;
using System.Runtime.InteropServices;
using G1ANT.Language;

namespace G1ANT.Addon.Twitter
{
    public static class SeleniumManager
    {
        private static SeleniumWrapper currentWrapper = null;

        public static SeleniumWrapper CurrentWrapper
        {
            get
            {
                if (currentWrapper == null)
                {
                    throw new InvalidOperationException("No Selenium browser instance attached. Please, use selenium.open command first.");
                }
                return currentWrapper;
            }
            private set
            {
                currentWrapper = value;
            }
        }

        private static List<SeleniumWrapper> wrappers = new List<SeleniumWrapper>();

        private static BrowserType GetBrowserType(string webBrowserName)
        {
            BrowserType? type = null;
            switch (webBrowserName.ToLower())
            {
                case "ie":
                case "iexplorer":
                case "internetexplorer":
                    type = BrowserType.InternetExplorer;
                    break;
                case "chrome":
                    type = BrowserType.Chrome;
                    break;
                case "firefox":
                case "ff":
                    type = BrowserType.Firefox;
                    break;
                case "edge":
                    type = BrowserType.Edge;
                    break;
            }
            if (type == null)
            {
                throw new ArgumentException("Invalid browsers argument. It accepts one of following values: 'ie', 'chrome', 'firefox', 'edge'");
            }
            return (BrowserType)type;
        }

        public static SeleniumWrapper CreateWrapper(string webBrowserName, string url, TimeSpan timeout, bool noWait, AbstractLogger scr, string driversDirectory)
        {
            IntPtr mainWindowHandle = IntPtr.Zero;
            BrowserType type = GetBrowserType(webBrowserName);
            if (type == BrowserType.Edge && wrappers.Where(x => x.BrowserType == BrowserType.Edge).Any())
            {
                throw new ApplicationException("Using multiple Edge instances at once is not supported.");
            }
            var driver = CreateNewWebDriver(webBrowserName, type, out mainWindowHandle, driversDirectory);
            var wrapper = new SeleniumWrapper(driver, mainWindowHandle, type, scr)
            {
                Id = wrappers.Count > 0 ? wrappers.Max(x => x.Id) + 1 : 0
            };
            wrappers.Add(wrapper);
            CurrentWrapper = wrapper;
            if (!string.IsNullOrEmpty(url))
            {
                CurrentWrapper.Navigate(url, timeout, noWait);
            }
            return CurrentWrapper;
        }

        public static void QuitCurrentWrapper()
        {
            if (CurrentWrapper != null)
            {
                CurrentWrapper.Quit();
                wrappers.Remove(CurrentWrapper);
                CurrentWrapper = null;
            }
        }

        public static void CleanUp()
        {
            var tempFolder = Path.GetTempPath();
            var tempScopedFolders = Directory.GetDirectories(tempFolder, "scoped_dir*", SearchOption.TopDirectoryOnly);
            foreach (var scopedFolder in tempScopedFolders)
            {
                try
                {
                    var directory = new DirectoryInfo(scopedFolder);
                    directory.Delete(true);
                }
                catch (Exception)
                {
                    //Ignored, as some of the scoped_dir are used by not closed selenium.
                }

            }

        }
        public static void Quit(SeleniumWrapper wrapper)
        {
            wrapper.Quit();
            RemoveWrapper(wrapper);
        }

        public static void RemoveWrapper(int id)
        {
            var toRemove = wrappers.Where(x => x.Id == id).FirstOrDefault();
            RemoveWrapper(toRemove);
        }

        public static void RemoveWrapper(SeleniumWrapper wrapper)
        {
            wrappers.Remove(wrapper);
            wrapper = null;
        }

        public static void DisposeAllOpenedDrivers()
        {
            List<Process> allDrivers = new List<Process>();
            List<Process> firefoxDrivers = Process.GetProcessesByName("geckodriver").ToList();
            List<Process> edgeDrivers = Process.GetProcessesByName("MicrosoftWebDriver").ToList();
            List<Process> ieDrivers = Process.GetProcessesByName("IEDriverServer").ToList();
            List<Process> chromeDrivers = Process.GetProcessesByName("chromedriver").ToList();

            allDrivers.AddRange(firefoxDrivers);
            allDrivers.AddRange(edgeDrivers);
            allDrivers.AddRange(ieDrivers);
            allDrivers.AddRange(chromeDrivers);

            foreach (Process process in allDrivers)
            {
                try
                {
                    process.Kill();
                }
                catch { }
            }
        }

        private static IWebDriver CreateNewWebDriver(string webBrowserName, BrowserType type, out IntPtr mainWindowHandle, string driversDirectory)
        {
            webBrowserName = webBrowserName.ToLower();
            IWebDriver iWebDriver = null;
            List<Process> processesBeforeLaunch = GetProcesses();
            string newProcessFilter = string.Empty;
            switch (type)
            {
                case BrowserType.Chrome:
                    iWebDriver = CreateChromeDriver(driversDirectory);
                    newProcessFilter = "chrome";
                    break;

                case BrowserType.Firefox:
                    iWebDriver = CreateFireFoxDriver(driversDirectory);
                    newProcessFilter = "firefox";
                    break;

                case BrowserType.InternetExplorer:
                    iWebDriver = CreateIEDriver(driversDirectory);
                    newProcessFilter = "iexplore";
                    break;

                case BrowserType.Edge:
                    iWebDriver = CreateEdgeWebDriver();
                    newProcessFilter = "edge";
                    break;
                default:
                    throw new ArgumentException($"Could not launch specified browser '{webBrowserName}'");
            }
            var newProcess = GetNewlyCreatedProcesses(newProcessFilter, processesBeforeLaunch);
            mainWindowHandle = (newProcess != null) ? newProcess.MainWindowHandle : IntPtr.Zero;
            return iWebDriver;
        }

        private static IWebDriver CreateChromeDriver(string driversDirectory)
        {
            var chromeService = Chrome.ChromeDriverService.CreateDefaultService(driversDirectory);
            chromeService.HideCommandPromptWindow = true;
            var chromeOptions = new Chrome.ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.None
            };
            chromeOptions.AddArgument("disable-infobars");
            chromeOptions.AddArgument("--disable-bundled-ppapi-flash");
            chromeOptions.AddArgument("--log-level=3");
            chromeOptions.AddArgument("--silent");
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("auto-open-devtools-for-tabs", false);
            return new Chrome.ChromeDriver(chromeService, chromeOptions);
        }

        private static IWebDriver CreateFireFoxDriver(string driversDirectory)
        {
            var firefoxService = Firefox.FirefoxDriverService.CreateDefaultService(driversDirectory);
            firefoxService.HideCommandPromptWindow = true;
            return new Firefox.FirefoxDriver(firefoxService);
        }

        private static IWebDriver CreateIEDriver(string driversDirectory)
        {
            IE.InternetExplorerDriverService ieService = IE.InternetExplorerDriverService.CreateDefaultService(driversDirectory);
            ieService.HideCommandPromptWindow = true;
            IE.InternetExplorerOptions options = new IE.InternetExplorerOptions()
            {
                IgnoreZoomLevel = true
            };
            return new IE.InternetExplorerDriver(ieService, options);
        }

        private static IWebDriver CreateEdgeWebDriver()
        {
            try
            {
                var edgeService = Edge.EdgeDriverService.CreateDefaultService();
                edgeService.HideCommandPromptWindow = true;
                var edgeOptions = new Edge.EdgeOptions
                {
                    PageLoadStrategy = PageLoadStrategy.Eager
                };
                return new Edge.EdgeDriver(edgeService, edgeOptions);
            }
            catch (DriverServiceNotFoundException ex)
            {
                throw new DriverServiceNotFoundException("To install run the following in an command prompt with admin privileges:\nDISM.exe /Online /Add-Capability /CapabilityName:Microsoft.WebDriver~~~~0.0.1.0", ex);
            }
        }

        public static void Switch(int id)
        {
            SeleniumWrapper pointedWrapper = wrappers.Where(x => x.Id == id).FirstOrDefault();
            CurrentWrapper = pointedWrapper ?? throw new InvalidOperationException($"Selenium browser instance with id '{id}' does not exist");
            CurrentWrapper.BringWindowToForeground();
        }

        private static List<Process> GetProcesses()
        {
            return Process.GetProcesses().ToList();
        }

        private static Process GetNewlyCreatedProcesses(string filter, List<Process> oldList)
        {
            return Process.GetProcesses().ToList()
                .Where(x => oldList.Where(y => y.Id == x.Id).Count() == 0 &&
                       x.MainWindowHandle != IntPtr.Zero &&
                       !string.IsNullOrEmpty(x.MainWindowTitle) &&
                       x.ProcessName.ToLower().Contains(filter.ToLower())).Where(x => IsWindowVisible(x.MainWindowHandle)).OrderBy(x => x.StartTime).ToList().FirstOrDefault();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
    }
}
