using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.MessengerApp
{
    [Command(Name = "messengerapp.typemessage", Tooltip = "This command is used to send message of any friend on Messenger App.")]
    public class MessengerAppTypeMessageCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Type text here which you want to send.")]
            public TextStructure Text { get; set; } = new TextStructure("");


            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("id");
        }

        public MessengerAppTypeMessageCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {

            arguments.Search.Value = "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[4]/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout[1]/android.widget.FrameLayout/android.widget.LinearLayout[2]/android.widget.EditText";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1000);
            arguments.Search.Value = "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[4]/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout[1]/android.widget.FrameLayout/android.widget.LinearLayout[1]/android.widget.EditText";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value, arguments.Search.Value).SendKeys(arguments.Text.Value);
            Thread.Sleep(2000);
            arguments.Search.Value = "1018, 980";
            arguments.By.Value = "xy";
            TouchAction clickAction = new TouchAction(OpenCommand.GetDriver());
            var coordinates = arguments.Search.Value.Split(',');
            clickAction.Tap(int.Parse(coordinates[0]), int.Parse(coordinates[1])).Perform();
            Thread.Sleep(3000);










        }
    }
}