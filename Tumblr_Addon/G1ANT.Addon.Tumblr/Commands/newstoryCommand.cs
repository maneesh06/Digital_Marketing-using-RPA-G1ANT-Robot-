using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using G1ANT.Language;

namespace G1ANT.Addon.Tumblr
{

    [Command(Name = "tumblr.newstory", Tooltip = "This command is used to add a new story on tumblr platform.")]
    public class newstoryCommand : Command
    {
        public newstoryCommand(AbstractScripter scripter) : base(scripter)
        {

        }
        public class Arguments : SeleniumCommandArguments
        {
            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            [Argument(Required = true, Tooltip = "Enter the title of story")]
            public TextStructure titlevalue { get; set; }
            [Argument(Required = true, Tooltip = "Enter story here")]
            public TextStructure storyvalue { get; set; }
            [Argument(DefaultVariable = "timeoutselenium", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(SeleniumSettings.SeleniumTimeout);
        }
        public void Execute(Arguments arguments)
        {
            try
            {
                SeleniumManager.CurrentWrapper.Navigate("https://tumblr.com/", arguments.Timeout.Value, arguments.NoWait.Value);
                Thread.Sleep(500);                
                arguments.Search.Value = "/html/body/div/div[1]/div[1]/div[2]/div[1]/main/div[3]/ul/li[1]/button";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div[3]/div/div/div/div/div[2]/div[2]/div/div[2]/div/div[1]/div/div/div[1]";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                SeleniumManager.CurrentWrapper.TypeText(arguments.titlevalue.Value, arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div[3]/div/div/div/div/div[2]/div[2]/div/div[2]/div/div[3]/div[1]/div/div[1]";
                arguments.By.Value = "xpath";
                SeleniumManager.CurrentWrapper.Click(arguments, arguments.Timeout.Value);
                SeleniumManager.CurrentWrapper.TypeText(arguments.storyvalue.Value, arguments, arguments.Timeout.Value);
                arguments.Search.Value = "/html/body/div[3]/div/div/div/div/div[2]/div[2]/div/div[5]/div[1]/div/div[3]/div/div/button";
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
