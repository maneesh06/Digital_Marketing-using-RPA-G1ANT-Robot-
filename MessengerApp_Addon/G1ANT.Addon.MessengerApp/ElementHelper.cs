using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;

namespace G1ANT.Addon.MessengerApp
{
    public static class ElementHelper
    {
        public static AndroidElement GetElement(string by, string name)
        {
            switch ((SearchBy)Enum.Parse(typeof(SearchBy), by, true))
            {
                case SearchBy.Id:
                    return GetElementById(name);
                case SearchBy.AccessibilityId:
                    return GetElementByAccessibilityId(name);
                case SearchBy.PartialId:
                    return GetElementByPartialId(name);
                case SearchBy.Text:
                    return GetElementByText(name);
                case SearchBy.Xpath:
                    return GetElementByXpath(name);
                default:
                    throw new ArgumentException($"Search criteria is invalid");
            }
        }

        private static AndroidElement GetElementById(string id)
        {
            var driver = OpenCommand.GetDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
                return driver.FindElementById(id);
            }
            catch
            {
                throw new ArgumentException($"Element with provided id was not found.");
            }
        }

        private static AndroidElement GetElementByAccessibilityId(string accessibilityId)
        {
            var driver = OpenCommand.GetDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@content-desc,\"" + accessibilityId + "\")]")));
                return driver.FindElementByAccessibilityId(accessibilityId);
            }
            catch
            {
                throw new ArgumentException($"Element with provided accessibility id was not found.");
            }
        }

        private static AndroidElement GetElementByPartialId(string partialId)
        {
            var driver = OpenCommand.GetDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@content-desc,\"" + partialId + "\")]")));
                return driver.FindElement(By.XPath("//*[contains(@content-desc,\"" + partialId + "\")]"));
            }
            catch
            {
                throw new ArgumentException($"Element with provided partial id was not found.");
            }
        }

        private static AndroidElement GetElementByText(string text)
        {
            var driver = OpenCommand.GetDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),\"" + text + "\")]")));
                return driver.FindElement(By.XPath("//*[contains(text(),\"" + text + "\")]"));
            }
            catch
            {
                throw new ArgumentException($"Element with provided text was not found.");
            }
        }

        private static AndroidElement GetElementByXpath(string xpath)
        {
            var driver = OpenCommand.GetDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
                return driver.FindElement(By.XPath(xpath));
            }
            catch
            {
                throw new ArgumentException($"Element with provided xpath was not found.");
            }
        }
    }
}