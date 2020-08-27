using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1ANT.Language;
using System.Threading;
namespace G1ANT.Addon.medium
{
    [Command(Name = "medium.googlelogin", Tooltip = "This command login medium account through google platform")]
    public class googleloginCommand : Command
    {
        public googleloginCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public class Arguments : SeleniumCommandArguments
        {
            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            [Argument(Required = true, Tooltip = "Enter the email")]
            public TextStructure email { get; set; }
            [Argument(Required = true, Tooltip = "Enter the email")]
            public TextStructure pword { get; set; }
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
                arguments.Search.Value = "/html/body/div[2]/div/div/div/div[2]/div/div/div/div/div[3]/div[1]/a/div";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "identifierId";
                arguments.By.Value = "id";                
                SeleniumManager.CurrentWrapper.TypeText(arguments.email.Value, arguments, arguments.Timeout.Value);
                SeleniumManager.CurrentWrapper.PressKey(
                    "enter",
                    arguments,
                    arguments.Timeout.Value);
                arguments.Search.Value = "password";
                arguments.By.Value = "name";
                SeleniumManager.CurrentWrapper.TypeText(arguments.pword.Value, arguments, arguments.Timeout.Value);
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