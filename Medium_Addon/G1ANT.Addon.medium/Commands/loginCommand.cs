using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1ANT.Language;
using System.Threading;
namespace G1ANT.Addon.medium
{
    [Command(Name = "medium.login", Tooltip = "This command login medium account")]
    public class loginCommand : Command
    {
        public loginCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public class Arguments : SeleniumCommandArguments
        {
            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            [Argument(Required = true, Tooltip = "Enter the email")]
            public TextStructure email { get; set; }
            [Argument(DefaultVariable = "timeoutselenium", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(SeleniumSettings.SeleniumTimeout);
        }
        public void Execute(Arguments arguments)
        {
            try
            {
                SeleniumManager.CurrentWrapper.Navigate("https://medium.com/", arguments.Timeout.Value, arguments.NoWait.Value);
                Thread.Sleep(500);
                arguments.Search.Value = "/html/body/div/div/div[4]/div/div/div/div/div[3]/div[3]/h4/span/a";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "email-susi-button-text";
                arguments.By.Value = "id";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div[2]/div/div/div/div[2]/div/div/div[3]/div/div[1]/div/div/div[2]/div/h4/input";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                SeleniumManager.CurrentWrapper.TypeText(arguments.email.Value, arguments, arguments.Timeout.Value);
                SeleniumManager.CurrentWrapper.PressKey(
                    "enter",
                    arguments,
                    arguments.Timeout.Value);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while opening new selenium instance. Message: {ex.Message}", ex);
            }
        }
    }
}