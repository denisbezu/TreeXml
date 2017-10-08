using System;
using System.Collections.Generic;

namespace TreeXml.Commands
{
    public class ExitCommand : ConsoleCommand
    {
        public override string ExecuteHelpCommand()// команда помощи
        {
            return "EXIT \t This command allows you to exit the program\n";
        }

        protected override bool ExecuteSpecialCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 1)
            {
                Environment.Exit(0);
            }
            return false;
        }
    }
}