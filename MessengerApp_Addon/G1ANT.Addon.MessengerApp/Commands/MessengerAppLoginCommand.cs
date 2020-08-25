using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.MessengerApp
{
    [Command(Name = "messengerapp.login", Tooltip = "This command login in the Messenger app.")]
    public class MessengerAppLoginCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = false, Tooltip = "Give your Email Id here.")]
            public TextStructure Email { get; set; } = new TextStructure("");
            [Argument(Required = false, Tooltip = "Give your Password here.")]
            public TextStructure Pass { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("id");
        }

        public MessengerAppLoginCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {
            arguments.Search.Value = "CONTINUE AS MANEESH";
            arguments.By.Value = "accessibilityid";
            var by = arguments.By.Value.ToLower();
            ElementHelper.GetElement(by, arguments.Search.Value).Click();
            Thread.Sleep(5000);
            arguments.Search.Value = "NOT NOW";
            arguments.By.Value = "accessibilityid";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1500);           
            arguments.Search.Value = "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout[2]/android.widget.LinearLayout/android.widget.Button[2]";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1500);
            arguments.Search.Value = "NOT NOW";
            arguments.By.Value = "accessibilityid";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(6000);
           







        }
    }
}