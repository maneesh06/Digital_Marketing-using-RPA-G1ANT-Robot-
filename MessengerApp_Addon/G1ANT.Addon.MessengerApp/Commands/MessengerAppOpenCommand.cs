using System;
using System.Diagnostics;
using System.Windows.Forms;
using G1ANT.Language;
using System.Threading;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.MessengerApp
{
    [Command(Name = "messengerapp.open", Tooltip = "This command initialises (open) Messenger app.")]
    public class OpenCommand : Language.Command
    {
        private static AndroidDriver<AndroidElement> driver;

        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "AppActivity")]
            public TextStructure AppActivity { get; set; } = new TextStructure("com.facebook.orca.auth.StartScreenActivity");

            [Argument(Required = true, Tooltip = "App Package")]
            public TextStructure AppPackage { get; set; } = new TextStructure("com.facebook.orca");

            [Argument(Required = false, Tooltip = "Automation Name")]
            public TextStructure AutomationName { get; set; } = new TextStructure("UiAutomator2");

            [Argument(Required = true, Tooltip = "Device Name")]
            public TextStructure DeviceName { get; set; } = new TextStructure("Android");

            [Argument(Required = true, Tooltip = "Platform Name")]
            public TextStructure PlatformName { get; set; } = new TextStructure("Android");

            [Argument(Required = false, Tooltip = "Platform version")]
            public TextStructure PlatformVersion { get; set; } = new TextStructure("");
        }

        public OpenCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            Initialize(arguments);
            Thread.Sleep(12000);
        }

        private AppiumOptions CreateAppiumOptions(Arguments arguments)
        {
            var desiredCapabilities = new AppiumOptions();
            desiredCapabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, arguments.DeviceName.Value);
            desiredCapabilities.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, arguments.AppPackage.Value);
            desiredCapabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, arguments.PlatformName.Value);
            desiredCapabilities.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, arguments.AppActivity.Value);
            if (!string.IsNullOrEmpty(arguments.PlatformVersion.Value))
            {
                desiredCapabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, arguments.PlatformVersion.Value);
            }
            return desiredCapabilities;
        }
        private void Initialize(Arguments arguments)
        {
            try
            {
                var appiumServiceBuilder = new AppiumServiceBuilder().UsingAnyFreePort();
                var appiumOptions = CreateAppiumOptions(arguments);
                driver = new AndroidDriver<AndroidElement>(appiumServiceBuilder, appiumOptions);
            }
            catch (Exception ex)
            {
                InstallAppiumWhenExceptionOccured(ex);
            }
        }

        private void InstallAppiumWhenExceptionOccured(Exception ex)
        {
            if (ex.Message.StartsWith("Invalid"))
            {
                var result = RobotMessageBox.Show("It seems you have no Appium driver installed. Would you like to install it now?", "Error", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Process.Start("\"C:\\Program Files\\nodejs\\npm.cmd\"", "install -g appium");
                }
            }
            else { throw ex; }
        }

        public static AndroidDriver<AndroidElement> GetDriver()
        {
            return driver;
        }
    }
}
