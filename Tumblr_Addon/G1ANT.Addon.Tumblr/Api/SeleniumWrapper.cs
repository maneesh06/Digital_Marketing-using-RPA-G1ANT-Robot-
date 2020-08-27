/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Selenium
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Addon.Tumblr.Api.Enums;
using G1ANT.Language;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data;
using System.Linq;

namespace G1ANT.Addon.Tumblr
{
    public class SeleniumWrapper
    {
        private IWebDriver webDriver = null;
        private readonly int scriptSeconds = 4;
        AbstractLogger logger = null;
        public string GetCurrentUrl => webDriver.Url;

        public class NewPopupWindowHandler
        {
            protected IWebDriver webDriver = null;
            protected int initialWindowHandlesCount = 0;

            public NewPopupWindowHandler(IWebDriver driver)
            {
                webDriver = driver;
                initialWindowHandlesCount = webDriver.WindowHandles.Count;
            }

            public void Finish(bool waitForNewWindow = false, TimeSpan timeout = new TimeSpan())
            {
                try
                {
                    if (waitForNewWindow == true)
                    {
                        var wait = new WebDriverWait(webDriver, timeout);
                        wait.Until(driver =>
                        {
                            return driver.WindowHandles.Count != initialWindowHandlesCount;
                        });
                        webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
                        if (webDriver.WindowHandles.Count > initialWindowHandlesCount)
                        {
                            wait = new WebDriverWait(webDriver, timeout);
                            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                        }
                    }
                    else if (webDriver.WindowHandles.Count != initialWindowHandlesCount)
                    {
                        webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
                        if (webDriver.WindowHandles.Count > initialWindowHandlesCount)
                        {
                            var wait = new WebDriverWait(webDriver, timeout);
                            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                        }
                    }
                }
                catch
                { }
            }
        }


        public BrowserType BrowserType { get; set; }

        public IntPtr MainWindowHandle { get; } = IntPtr.Zero;

        public int Id { get; set; }

        public string Title
        {
            get
            {
                return webDriver.Title;
            }
        }

        public SeleniumWrapper(IWebDriver webDriver, IntPtr mainWindowHandle, BrowserType type, AbstractLogger scr)
        {
            logger = scr;
            this.MainWindowHandle = mainWindowHandle;
            this.webDriver = webDriver;
            BrowserType = type;
            webDriver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 0, scriptSeconds);
        }

        private IWebElement FindElement(string search, string by, TimeSpan timeout)
        {
            IWebElement element = null;
            var searchBy = Enum.GetValues(typeof(ElementSearchBy))
                .Cast<ElementSearchBy?>()
                .FirstOrDefault(e => e.ToString().Equals(by, StringComparison.OrdinalIgnoreCase));

            try
            {
                switch (searchBy)
                {
                    case null:
                        throw new ArgumentException("Value for argument 'By' was not recognized");
                    case ElementSearchBy.Id:
                        search = search.StartsWith("#") ? search.TrimStart(new char[] { '#' }) : search;
                        element = new WebDriverWait(webDriver, timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(search)));
                        break;
                    case ElementSearchBy.Class:
                        element = new WebDriverWait(webDriver, timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName(search)));
                        break;
                    case ElementSearchBy.CssSelector:
                        element = new WebDriverWait(webDriver, timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(search)));
                        break;
                    case ElementSearchBy.Tag:
                        element = new WebDriverWait(webDriver, timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.TagName(search)));
                        break;
                    case ElementSearchBy.Xpath:
                        element = new WebDriverWait(webDriver, timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(search)));
                        break;
                    case ElementSearchBy.Name:
                        element = new WebDriverWait(webDriver, timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name(search)));
                        break;
                    case ElementSearchBy.Query:
                        element = new WebDriverWait(webDriver, timeout).Until(CustomExpectedConditions.ElementExistsByJavaScript(search));
                        break;
                    case ElementSearchBy.Jquery:
                        element = new WebDriverWait(webDriver, timeout).Until(CustomExpectedConditions.ElementExistsByJquery(search));
                        break;
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch
            {
                string errorMessage = $"Timeout occured while waiting for element. Search phrase: '{search}', by: '{by}' {(webDriver is InternetExplorerDriver ? ". It might be necessary to disable protection mode and lower security level in Internet Explorer." : string.Empty)}";
                throw new TimeoutException(errorMessage);
            }

            return element;
        }

        private static string ValidateUrl(string url)
        {
            try
            {
                return new UriBuilder(url).ToString();
            }
            catch
            {
                throw new UriFormatException($"Specified url '{url}' is in a wrong format.");
            }
        }

        private void PreCheckCurrentWindowHandle()
        {
            string found = null;
            try
            {
                found = webDriver.WindowHandles.Where(x => x == webDriver.CurrentWindowHandle).FirstOrDefault();
            }
            catch
            { }
            if (found == null)
            {
                webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            }
        }

        public void Navigate(string url, TimeSpan timeout, bool noWait)
        {
            url = ValidateUrl(url);
            if (!noWait)
            {
                webDriver.Navigate().GoToUrl(url);
                WebDriverWait wait = new WebDriverWait(webDriver, timeout);
                wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            else
            {
                try
                {
                    webDriver.JavaScriptExecutor().ExecuteAsyncScript($"window.location.href = '{url}';");
                }
                catch (Exception ex)
                {
                    logger.Log(AbstractLogger.Level.Error, $"Problem while navigating to url: '{url}' :  {ex.Message}");
                }
            }
        }

        public void Refresh()
        {
            webDriver.Navigate().Refresh();
        }

        public void Quit()
        {
            webDriver.Quit();
        }

        public void Dispose()
        {
            webDriver.Dispose();
        }

        public string RunScript(string script, TimeSpan timeout = new TimeSpan(), bool waitForNewWindow = false)
        {
            NewPopupWindowHandler popupHandler = new NewPopupWindowHandler(webDriver);
            PreCheckCurrentWindowHandle();
            script += "; return null;";
            object result = webDriver.JavaScriptExecutor().ExecuteScript(script);
            popupHandler.Finish(waitForNewWindow, timeout);
            return result?.ToString() ?? string.Empty;
        }

        public void CloseTab(TimeSpan timeout)
        {
            PreCheckCurrentWindowHandle();
            switch (BrowserType)
            {
                case BrowserType.Edge:
                case BrowserType.InternetExplorer:
                    throw new ApplicationException("CloseTab command is not supported by Edge and Internet Explorer selenium driver.");
                case BrowserType.Firefox:
                case BrowserType.Chrome:
                    RunScript(string.Format($"window.close();"));
                    break;
            }
            webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
        }

        public void NewTab(TimeSpan timeout, string url, bool noWait)
        {
            url = ValidateUrl(url);
            switch (BrowserType)
            {
                case BrowserType.Edge:
                case BrowserType.InternetExplorer:
                    throw new ApplicationException("NewTab command is not supported by Edge and Internet Explorer selenium driver.");
                case BrowserType.Firefox:
                case BrowserType.Chrome:
                    RunScript(string.Format($"window.open('','_blank');"));
                    break;
            }
            webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            if (!string.IsNullOrEmpty(url))
            {
                Navigate(url, timeout, noWait);
            }
        }

        public void ActivateTab(string phrase, string by)
        {
            if (BrowserType == BrowserType.Edge ||
                BrowserType == BrowserType.InternetExplorer)
            {
                throw new ApplicationException("Activating tabs in Edge and Internet Explorer is not supported.");
            }
            by = by.ToLower();
            phrase = phrase.ToLower();
            string originalHandler = webDriver.CurrentWindowHandle;
            foreach (var handler in webDriver.WindowHandles)
            {
                IWebDriver currentHandler = webDriver.SwitchTo().Window(handler);
                switch (by)
                {
                    case "url":
                        if (currentHandler.Url.ToLower().Contains(phrase))
                        {
                            return;
                        }
                        break;

                    case "title":
                        if (currentHandler.Title.ToLower().Contains(phrase))
                        {
                            return;
                        }
                        break;
                    default:
                        throw new ArgumentException("Argument 'by' not recognized. It accepts either 'title' or 'url' value.");
                }
            }
            webDriver.SwitchTo().Window(originalHandler);
            throw new ArgumentException($"Specified tab (by: '{by}, search: '{phrase}') not found.");
        }

        public void AlertPerformAction(string action, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, timeout);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            var alert = webDriver.SwitchTo().Alert();
            switch (action)
            {
                case "accept":
                    alert.Accept();
                    break;
                case "decline":
                    alert.Dismiss();
                    break;
                default:
                    throw new NotSupportedException($"Action '{action}' is not supported.");
            }
        }

        public void Click(SeleniumCommandArguments search, TimeSpan timeout, bool waitForNewWindow = false)
        {
            NewPopupWindowHandler popupHandler = new NewPopupWindowHandler(webDriver);
            var elem = GetElementInFrame(search, timeout);
            Actions actions = new Actions(webDriver);
            actions.MoveToElement(elem).Click().Build().Perform();
            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().DefaultContent();
            popupHandler.Finish(waitForNewWindow, timeout);
        }

        public void TypeText(string text, SeleniumCommandArguments search, TimeSpan timeout)
        {
            var elem = GetElementInFrame(search, timeout);
            elem.SendKeys(text);
            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().DefaultContent();
        }

        public void PressKey(string keyText, SeleniumCommandArguments search, TimeSpan timeout)
        {
            NewPopupWindowHandler popupHandler = new NewPopupWindowHandler(webDriver);
            var elem = GetElementInFrame(search, timeout);
            string convertedText = typeof(Keys).GetFields().Where(x => x.Name.ToLower() == keyText.ToLower()).FirstOrDefault()?.GetValue(null) as string;
            if (convertedText == null)
            {
                throw new ArgumentException($"Wrong key argument '{keyText}' specified. Please use keys allowed by selenium library.");
            }
            elem.SendKeys(convertedText);
            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().DefaultContent();
            popupHandler.Finish();
        }

        public void SetAttributeValue(string attributeName, string attributeValue, AttributeOperationType setAttributeType, SeleniumCommandArguments search, TimeSpan timeout)
        {
            var element = GetElementInFrame(search, timeout);
            if (element != null)
            {
                if (IsAttributeOprtationType(element, attributeName, setAttributeType))
                    element.SetAttribute(attributeName, attributeValue);
                else
                    element.SetProperty(attributeName, attributeValue);
            }
            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().DefaultContent();
        }

        private bool IsAttributeOprtationType(IWebElement element, string attributeName, AttributeOperationType setAttributeType)
        {
            return setAttributeType == AttributeOperationType.ForceAttribute
                || (setAttributeType == AttributeOperationType.PreferAttribute && element.IsAttribute(attributeName));
        }

        public string GetAttributeValue(string attributeName, SeleniumCommandArguments search)
        {
            var element = GetElementInFrame(search, search.Timeout.Value);

            var result = element?.GetAttribute(attributeName);

            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().DefaultContent();

            return result ?? string.Empty;
        }

        public string GetAttributeValue(string attributeName, string elementXPath, SeleniumIFrameArguments search)
        {
            var element = GetElementInFrame(new SeleniumCommandArguments()
            {
                By = new TextStructure("xpath"),
                IFrameBy = search.IFrameBy,
                IFrameSearch = search.IFrameSearch,
                Search = new TextStructure(elementXPath),
            }, search.Timeout.Value);
            var res = element?.GetAttribute(attributeName) ?? string.Empty;
            if (!string.IsNullOrEmpty(search.IFrameSearch?.Value))
                webDriver.SwitchTo().DefaultContent();
            return res;
        }

        public string GetHtml(SeleniumIFrameArguments search)
        {
            return GetAttributeValue("outerHTML", "//html", search);
        }

        public string GetInnerHtml(SeleniumCommandArguments search)
        {
            return GetAttributeValue("innerHTML", search);
        }

        public string GetOuterHtml(SeleniumCommandArguments search)
        {
            return GetAttributeValue("outerHTML", search);
        }

        public string GetTextValue(SeleniumCommandArguments search, TimeSpan timeout)
        {
            var element = GetElementInFrame(search, timeout);

            var res = element?.Text ?? string.Empty;
            if (!string.IsNullOrEmpty(search.IFrameSearch?.Value))
                webDriver.SwitchTo().DefaultContent();
            return res;
        }

        public DataTable GetTableElement(SeleniumCommandArguments search, TimeSpan timeout)
        {
            var element = GetElementInFrame(search, timeout);

            if (element == null)
                throw new Exception("Cannot find the HTML element. Try to change the search phrase or \"by\" argument value so that the correct element is found");
            else if (element.TagName != "table")
                throw new Exception($"The element found has the \"{element.TagName}\" tag name. Try to change the search phrase so that the \"table\" element is found instead");

            var dataTable = new DataTable();
            var trElements = element.FindElements(By.TagName("tr"));
            dataTable = AddColumnNamesFromThElements(dataTable, trElements[0].FindElements(By.TagName("th")));

            foreach (var trElement in trElements)
            {
                var tdElements = trElement.FindElements(By.TagName("td"));
                dataTable = AddColumnsIfThereAreMoreTdElements(dataTable, tdElements.Count);
                dataTable = AddRowToDataTable(dataTable, tdElements);
            }

            return dataTable;
        }

        private static DataTable AddColumnNamesFromThElements(DataTable dataTable, System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> thElements)
        {
            foreach (var thElement in thElements)
                dataTable.Columns.Add(thElement.Text);
            return dataTable;
        }

        private static DataTable AddColumnsIfThereAreMoreTdElements(DataTable dataTable, int tdElementsNumber)
        {
            while (dataTable.Columns.Count < tdElementsNumber)
                dataTable.Columns.Add();
            return dataTable;
        }

        private static DataTable AddRowToDataTable(DataTable dataTable, System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> tdElements)
        {
            var cellValues = new string[tdElements.Count];
            var i = 0;
            foreach (var tdElement in tdElements)
            {
                if (tdElement.GetAttribute("colspan") != null || tdElement.GetAttribute("rowspan") != null)
                    throw new Exception("This table contains merged cells which is unsupported. Make sure to choose a table that does not have any \"colspan\" or \"rowspan\" properties");
                cellValues[i] = tdElement.Text;
                i++;
            }
            dataTable.Rows.Add(cellValues);

            return dataTable;
        }

        private IWebElement GetElementInFrame(SeleniumCommandArguments search, TimeSpan timeout)
        {
            PreCheckCurrentWindowHandle();
            if (!string.IsNullOrEmpty(search.IFrameSearch?.Value))
                webDriver.SwitchTo().Frame(FindElement(search.IFrameSearch.Value, search.IFrameBy.Value, timeout));
            var element = FindElement(search.Search.Value, search.By.Value, timeout);
            return element;
        }

        public void CallFunction(string functionName, object[] arguments, string type, SeleniumCommandArguments search, TimeSpan timeout)
        {
            NewPopupWindowHandler popupHandler = new NewPopupWindowHandler(webDriver);
            PreCheckCurrentWindowHandle();
            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().Frame(FindElement(search.IFrameSearch.Value, search.IFrameBy.Value, timeout));
            IWebElement element = FindElement(search.Search.Value, search.By.Value, timeout);
            element?.CallFunction(functionName, arguments, type);
            if (string.IsNullOrEmpty(search.IFrameSearch?.Value) == false)
                webDriver.SwitchTo().DefaultContent();
            popupHandler.Finish();
        }

        public void BringWindowToForeground()
        {
            if (MainWindowHandle != IntPtr.Zero)
            {
                Language.RobotWin32.SetForegroundWindow(MainWindowHandle);
            }
        }
    }
}
