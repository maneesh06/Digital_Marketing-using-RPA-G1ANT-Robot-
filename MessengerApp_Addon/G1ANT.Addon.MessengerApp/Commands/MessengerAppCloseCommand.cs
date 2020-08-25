using G1ANT.Language;
using OpenQA.Selenium.Remote;

namespace G1ANT.Addon.MessengerApp
{
    [Command(Name = "messengerapp.close", Tooltip = "This command closes Messenger app")]
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