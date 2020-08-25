using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.QuoraApp
{
    [Command(Name = "quoraapp.askquestion", Tooltip = "This command post given question on Quora app")]
    public class QuoraAppAskQuestionCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Type question here which you want to post.")]
            public TextStructure Question { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("");
        }

        public QuoraAppAskQuestionCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {
            arguments.Search.Value = "958, 1515";
            arguments.By.Value = "xy";
            TouchAction clickAction = new TouchAction(OpenCommand.GetDriver());
            var coordinates = arguments.Search.Value.Split(',');
            clickAction.Tap(int.Parse(coordinates[0]), int.Parse(coordinates[1])).Perform();
            Thread.Sleep(2000);
            arguments.Search.Value = "161, 1232";
            arguments.By.Value = "xy";
            TouchAction clicAction = new TouchAction(OpenCommand.GetDriver());
            var cordinates = arguments.Search.Value.Split(',');
            clicAction.Tap(int.Parse(cordinates[0]), int.Parse(cordinates[1])).Perform();
            Thread.Sleep(1000);
            arguments.Search.Value = "318, 1537";
            arguments.By.Value = "xy";
            TouchAction clicActio = new TouchAction(OpenCommand.GetDriver());
            var cordinate = arguments.Search.Value.Split(',');
            clicActio.Tap(int.Parse(cordinate[0]), int.Parse(cordinate[1])).Perform();
            Thread.Sleep(1000);
            arguments.Search.Value = "//android.webkit.WebView[@content-desc='Quora']/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View[2]/android.view.View[2]/android.view.View[6]/android.view.View/android.view.View[1]/android.widget.EditText";
            arguments.By.Value = "xpath";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(500);
            ElementHelper.GetElement(arguments.By.Value, arguments.Search.Value).SendKeys(arguments.Question.Value);
            Thread.Sleep(200);
            arguments.Search.Value = "com.quora.android:id/ask_button";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1000);
            arguments.Search.Value = "Use suggestion";
            arguments.By.Value = "accessibilityid";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(2000);








        }
    }
}