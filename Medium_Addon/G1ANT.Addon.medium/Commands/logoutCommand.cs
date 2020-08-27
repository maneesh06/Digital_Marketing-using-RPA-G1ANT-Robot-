using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1ANT.Language;

namespace G1ANT.Addon.medium
{
    [Command(Name = "medium.logout", Tooltip = "This command is used to logout profile in medium")]
    public class logoutCommand : Command
    {
        public logoutCommand(AbstractScripter scripter) : base(scripter)
        {

        }
        public class Arguments : SeleniumCommandArguments
        {           
            [Argument(DefaultVariable = "timeoutselenium", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(SeleniumSettings.SeleniumTimeout);
        }

        public void Execute(Arguments arguments)
        {
            try
            {
                SeleniumWrapper wrapper = SeleniumManager.CreateWrapper(
                        "chrome",
                        "medium.com",
                        arguments.Timeout.Value,
                        false,
                        Scripter.Log,
                        Scripter.Settings.UserDocsAddonFolder.FullName);
                int wrapperId = wrapper.Id;
                OnScriptEnd = () =>
                {
                    SeleniumManager.DisposeAllOpenedDrivers();
                    SeleniumManager.RemoveWrapper(wrapperId);
                    SeleniumManager.CleanUp();
                };

                arguments.Search.Value = "/html/body/div[1]/div[2]/div/div[1]/div[2]/div[2]/div/button[2]";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div[1]/div[2]/div/div[1]/div[3]/div[1]/ul/li[17]/a";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);                
                var len = SeleniumManager.CurrentWrapper.RunScript("return document.getElementsByClassName(\"captcha -internal\").length");
                if (len == "1")
                {
                    RobotMessageBox.Show("Captcha detected, please solve the captcha");
                }
            }

            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while opening new selenium instance. Message: {ex.Message}", ex);
            }
        }
    }


}
