using TreeXml.Commands;
using TreeXml.Enum;

namespace TreeXml
{
    public class ConsoleFactory
    {
        public ConsoleCommand GetConsoleCommand(ECommand eCommand)
        {
            ConsoleCommand command = null;
            if(eCommand == ECommand.Open)
                command = new OpenCommand();
            else if(eCommand == ECommand.Clear)
                command =  new ClearCommand();
            else if(eCommand == ECommand.Exit)
                command =  new ExitCommand();
            else if (eCommand == ECommand.Help)
                command = new HelpCommand();
            else if (eCommand == ECommand.Connect)
                command = new ConnectCommand();
            else if(eCommand == ECommand.OpenDb)
                command = new OpenDbCommand();
            return command;
        }
    }
}