using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.FlipkartApp
{
    [Command(Name = "flipkartapp.search", Tooltip = "This command Search a given element in Flipkart app")]
    public class FlipkartAppSearchCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Type text here which you want to search.")]
            public TextStructure Text { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("id");
        }

        public FlipkartAppSearchCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {
            
            arguments.Search.Value = "com.flipkart.android:id/search_widget_textbox";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(500);
            arguments.Search.Value = "Search grocery products in Supermart";
            arguments.By.Value = "accessibilityid";
            ElementHelper.GetElement(arguments.By.Value, arguments.Search.Value).SendKeys(arguments.Text.Value);
            Thread.Sleep(100);
            arguments.Search.Value = "980, 1694";
            arguments.By.Value = "xy";
            TouchAction clickAction = new TouchAction(OpenCommand.GetDriver());
            var coordinates = arguments.Search.Value.Split(',');
            clickAction.Tap(int.Parse(coordinates[0]), int.Parse(coordinates[1])).Perform();
            Thread.Sleep(2000);








        }
    }
}