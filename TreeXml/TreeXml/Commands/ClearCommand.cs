using System;
using System.Collections.Generic;
using TreeXml.Interfaces;

namespace TreeXml.Commands
{
    public class ClearCommand : IConsoleCommand
    {
        public bool ExecuteCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 1 && commandArgs[0].ToLower().Equals("cls")) // второе условие возможно не нужно
            {
                Console.Clear();
                return true;
            }
            else if (commandArgs.Count == 2 && CheckCommand(commandArgs[1]))
            {
                Console.Write(ExecuteHelpCommand());
                return true;
            }
            return false;
        }

        public string ExecuteHelpCommand()
        {
            return "CLS \t This command allows you to clear the screen\n";
        }

        private bool CheckCommand(string parameter)
        {
            if (parameter == "/?")
                return true;
            return false;
        }
    }
}