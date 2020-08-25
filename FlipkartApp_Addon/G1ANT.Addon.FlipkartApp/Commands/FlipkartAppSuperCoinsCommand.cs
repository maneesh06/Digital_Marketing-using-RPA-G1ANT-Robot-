using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.FlipkartApp
{
    [Command(Name = "flipkartapp.supercoins", Tooltip = "This command helps you to show your super coins on Flipkart app")]
    public class FlipkartAppSuperCoinsCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {


            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("");
        }

        public FlipkartAppSuperCoinsCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {

            arguments.Search.Value = "//android.widget.FrameLayout[@content-desc='SuperCoin']/android.widget.ImageView";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(2000);


            







        }
    }
}