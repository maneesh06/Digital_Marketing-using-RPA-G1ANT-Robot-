using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using G1ANT.Language;

namespace G1ANT.Addon.medium
{

    [Command(Name = "medium.setting", Tooltip = "This command is used to change setting of medium platform.")]
    public class settingCommand : Command
    {
        public settingCommand(AbstractScripter scripter) : base(scripter)
        {

        }
        public class Arguments : SeleniumCommandArguments
        {
            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(SeleniumSettings.SeleniumTimeout);
        }
        public void Execute(Arguments arguments)
        {
            try
            {
                SeleniumManager.CurrentWrapper.Navigate("https://medium.com/me/settings", arguments.Timeout.Value, arguments.NoWait.Value);
                Thread.Sleep(500);                
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while opening new selenium instance. Message: {ex.Message}", ex);
            }



        }


    }
}
