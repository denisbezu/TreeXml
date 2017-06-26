using System;
using System.Collections.Generic;
using TreeXml.Interfaces;

namespace TreeXml.Commands
{
    public class ExitCommand : IConsoleCommand
    {
        public bool ExecuteCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 1 && commandArgs[0].ToLower().Equals("exit")) // второе условие возможно не нужно
            {
                Environment.Exit(0);
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
            return "EXIT \t This command allows you to exit the program\n";
        }

        private bool CheckCommand(string parameter)
        {
            if (parameter == "/?")
                return true;
            return false;
        }
    }
}