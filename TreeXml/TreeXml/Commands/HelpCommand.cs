using System;
using System.Collections.Generic;

namespace TreeXml.Commands
{
    public class HelpCommand : ConsoleCommand
    {
        public override string ExecuteHelpCommand() // помощь 
        {
            return "HELP \t This command allows you to view all available commands\n";
        }

        protected override bool ExecuteSpecialCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 1 && commandArgs[0].ToLower().Equals("help")) // второе условие возможно не нужно
            {
                Console.Write("Available commands:\n" +
                              "open \t open the tree " +
                              "\nhelp \t Show this help " +
                              "\nexit \t close program" +
                              "\ncls \t clear console" +
                              "\nopendb \t open database" +
                              "\nconnect \t connect to the SQL server");
                return true;
            }
            return false;
        }
    }
}
