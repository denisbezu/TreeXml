using System;
using System.Collections.Generic;

namespace TreeXml.Commands
{
    public class ClearCommand : ConsoleCommand
    {
        public override string ExecuteHelpCommand()//отображение помощи
        {
            return "CLS \t This command allows you to clear the screen\n";
        }

        protected override bool ExecuteSpecialCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 1)
            {
                Console.Clear();
                return true;
            }
            return false;
        }
    }
}