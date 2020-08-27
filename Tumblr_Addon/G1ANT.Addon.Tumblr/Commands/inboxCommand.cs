using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1ANT.Language;
using System.Threading;
namespace G1ANT.Addon.Tumblr
{
    [Command(Name = "tumblr.inbox", Tooltip = "This command open inbox of messages recieved on tumblr platfom ")]
    public class inboxCommand : Command
    {
        public inboxCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        public class Arguments : SeleniumCommandArguments
        {            
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            [Argument(DefaultVariable = "timeoutselenium", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(SeleniumSettings.SeleniumTimeout);
        }
        public void Execute(Arguments arguments)
        {
            try
            {
                SeleniumManager.CurrentWrapper.Navigate("https://tumblr.com/inbox", arguments.Timeout.Value, arguments.NoWait.Value);
                Thread.Sleep(500);               

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while opening new selenium instance. Message: {ex.Message}", ex);
            }
        }
    }
}