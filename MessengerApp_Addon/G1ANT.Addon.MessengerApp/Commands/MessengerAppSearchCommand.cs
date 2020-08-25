using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.MessengerApp
{
    [Command(Name = "messengerapp.search", Tooltip = "This command Search a given friend in Messenger app")]
    public class MessengerAppSearchCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Type text here which you want to search.")]
            public TextStructure Friend { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("id");
        }

        public MessengerAppSearchCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {
            
            arguments.Search.Value = "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.FrameLayout/android.view.ViewGroup/androidx.recyclerview.widget.RecyclerView/android.view.ViewGroup[2]/android.view.ViewGroup";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1000);
            arguments.Search.Value = "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[3]/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.view.ViewGroup/androidx.appcompat.widget.LinearLayoutCompat/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.EditText";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value, arguments.Search.Value).SendKeys(arguments.Friend.Value);
            Thread.Sleep(200);
            arguments.Search.Value = "1001, 1701";
            arguments.By.Value = "xy";
            TouchAction clickAction = new TouchAction(OpenCommand.GetDriver());
            var coordinates = arguments.Search.Value.Split(',');
            clickAction.Tap(int.Parse(coordinates[0]), int.Parse(coordinates[1])).Perform();
            Thread.Sleep(3000);








        }
    }
}