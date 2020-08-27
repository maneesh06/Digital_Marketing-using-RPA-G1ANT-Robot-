/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Selenium
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;

namespace G1ANT.Addon.Tumblr
{
    public static class SeleniumExtensions
    {
        public static IJavaScriptExecutor JavaScriptExecutor(this IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }


        /// <summary>Sets attribute of an element (the one from element.attributes)</summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IWebElement SetAttribute(this IWebElement element, string name, string value)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", element, name, value);
            return element;
        }

        public static bool IsAttribute(this IWebElement element, string name)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;
            var isAttribute = name != "innerHTML" // getAttribute returns "innerHTML" for this name while it should return null
                && (bool)jsExecutor.ExecuteScript("return arguments[0].getAttribute(arguments[1]) != null", element, name);

            return isAttribute;
            
        }

        /// <summary>Sets property of an element (element.name = value), handles cases like `value` or `selectedIndex`</summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IWebElement SetProperty(this IWebElement element, string name, string value)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;

            if (!int.TryParse(value, out _))
                value = $"'{value.Replace("'", "\\'")}'";

            jsExecutor.ExecuteScript($"arguments[0].{name} = {value};", element);
            return element;
        }

        public static IWebElement CallFunction(this IWebElement element, string functionName, object[] arguments, string type)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;
            string argumentsToPass = string.Empty;
            for (int i = 1; i <= arguments.Length; i++)
            {
                argumentsToPass += $"arguments[{i}]";
                if (i < arguments.Length)
                {
                    argumentsToPass += ", ";
                }
            }

            string jsCode = string.Empty;
            switch (type.ToLower())
            {
                case "javascript":
                    jsCode = $"arguments[0].{functionName}({argumentsToPass});";
                    break;

                case "jquery":
                    jsCode = $"$(arguments[0]).{functionName}({argumentsToPass});";
                    break;
                default:
                    throw new ArgumentException("'Type' argument accepts either 'javascript' or 'jquery' value");

            }
            jsExecutor.ExecuteScript(jsCode, element, arguments);
            return element;
        }
    }
}
