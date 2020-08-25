using System;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.FlipkartApp
{
    [Command(Name = "flipkartapp.login", Tooltip = "This command login in the Flipkart app.")]
    public class FlipkartAppLoginCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Give your Email Id here.")]
            public TextStructure Email { get; set; } = new TextStructure("");
            [Argument(Required = true, Tooltip = "Give your Password here.")]
            public TextStructure Pass { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide name of the capaility")]
            public TextStructure Search { get; set; } = new TextStructure("");

            [Argument(Required = false, Tooltip = "Provide element ID")]
            public TextStructure By { get; set; } = new TextStructure("id");
        }

        public FlipkartAppLoginCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {
            arguments.Search.Value = "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.RelativeLayout/android.widget.LinearLayout[1]/androidx.recyclerview.widget.RecyclerView/android.widget.RelativeLayout[5]/android.widget.RelativeLayout";
            arguments.By.Value = "xpath";
            var by = arguments.By.Value.ToLower();
            ElementHelper.GetElement(by, arguments.Search.Value).Click();
            Thread.Sleep(200);
            arguments.Search.Value = "com.flipkart.android:id/select_btn";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1000);
            arguments.Search.Value = "com.google.android.gms:id/cancel";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(200);
            arguments.Search.Value = "com.flipkart.android:id/tv_right_cta";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(1000);
            arguments.Search.Value = "Email ID";
            arguments.By.Value = "accessibilityid";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).SendKeys(arguments.Email.Value);
            Thread.Sleep(10);
            arguments.Search.Value = "com.flipkart.android:id/button";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(2000);
            arguments.Search.Value = "com.flipkart.android:id/phone_input";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value, arguments.Search.Value).SendKeys(arguments.Pass.Value);
            Thread.Sleep(100);           
            arguments.Search.Value = "com.flipkart.android:id/button";
            arguments.By.Value = "id";
            ElementHelper.GetElement(arguments.By.Value.ToLower(), arguments.Search.Value).Click();
            Thread.Sleep(7000);







        }
    }
}