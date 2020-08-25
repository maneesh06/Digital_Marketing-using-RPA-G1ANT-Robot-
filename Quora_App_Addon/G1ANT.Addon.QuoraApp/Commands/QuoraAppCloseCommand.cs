using G1ANT.Language;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.QuoraApp
{
    [Command(Name = "quoraapp.close", Tooltip = "This command closes Quora app")]
    public class CloseCommand : Language.Command
    {
        public class Arguments : CommandArguments
        {

        }

        public CloseCommand(AbstractScripter scripter) : base(scripter)
        {

        }

        public void Execute(Arguments arguments)
        {
            var driver = OpenCommand.GetDriver();
            driver.Quit();
        }
    }
}