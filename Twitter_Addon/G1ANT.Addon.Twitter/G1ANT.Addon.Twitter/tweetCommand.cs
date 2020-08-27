using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using G1ANT.Language;

namespace G1ANT.Addon.Twitter

{
    [Command(Name ="twitter.tweet", Tooltip ="This command use to tweet on Twitter")]
    class tweetCommand:Command 
    {
        public tweetCommand(AbstractScripter scripter) : base(scripter)
        {

        }
        public class Arguments : SeleniumCommandArguments
        {
           [Argument(Required = true, Tooltip = "Enter the value to be posted ")]
            public TextStructure MessageValue { get; set; }

        }
        public void Execute(Arguments arguments)
        { 
            try
            {
                //arguments.Search.Value = "bug-text-color";
                //arguments.By.Value = "class";
                //SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div/div/div/div[2]/main/div/div/div/div/div/div[2]/div/div[2]/div[1]/div/div/div/div[2]/div[1]/div/div/div/div/div/div/div/div/div/div[1]/div";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div/div/div/div[2]/header/div/div/div/div[1]/div[3]/a/div";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.TypeText(arguments.MessageValue.Value, arguments, arguments.Timeout.Value);
                Thread.Sleep(2000);
                             
                
                

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while opening new selenium instance. Message: {ex.Message}", ex);
            }
        }
    }
}
