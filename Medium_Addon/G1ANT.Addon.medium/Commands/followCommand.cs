using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1ANT.Language;

namespace G1ANT.Addon.medium
{

    [Command(Name = "medium.follow", Tooltip = "This command is used to follow our interest on medium platform.")]
    public class followCommand : Command
    {
        public followCommand(AbstractScripter scripter) : base(scripter)
        {

        }
        public class Arguments : SeleniumCommandArguments
        {           
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(SeleniumSettings.SeleniumTimeout);
        }
        public void Execute(Arguments arguments)
        {
            try
            {
                arguments.Search.Value = "/html/body/div[1]/div[2]/div/div[3]/div[2]/div/div/div/div[2]/div/div/div/div[2]/div[2]/span/button[2]";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);                
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while opening new selenium instance. Message: {ex.Message}", ex);
            }



        }


    }
}
