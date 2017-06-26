using System;
using System.Collections.Generic;
using System.Text;
using TreeXml.Interfaces;

namespace TreeXml.Commands
{
    public class HelpCommand : IConsoleCommand
    {
        public bool ExecuteCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 1 && commandArgs[0].ToLower().Equals("help")) // второе условие возможно не нужно
            {
                Console.Write("Available commands:\n " +
                              "-s \t Show the tree \nhelp \t Show this help \nexit \t" +
                              " close program\ncls \t clear console\n-a \t Search algorithm\n");
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
            return "HELP \t This command allows you to view all available commands\n";
        }

        private bool CheckCommand(string parameter)
        {
            if (parameter == "/?")
                return true;
            return false;
        }
    }
}