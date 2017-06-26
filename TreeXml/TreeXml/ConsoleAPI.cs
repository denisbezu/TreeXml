using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TreeXml.Commands;
using TreeXml.Interfaces;
using TreeXmlLibrary;

namespace TreeXml
{
    public class ConsoleApi
    {
        private string Input { get; set; }
        public void StartInput()
        {
            while (true)
            {
                Input = Console.ReadLine();
                var commandsParameters = SplitInput();
                if (commandsParameters.Count != 0)
                    if (!CheckInputCommand(commandsParameters))
                    {
                        InValidCommand();
                        Console.WriteLine();
                    }
            }
        }
        private void InValidCommand()
        {
            Console.Write("Invalid command, retry please");
        }
        private bool CheckInputCommand(List<string> commandsParameters)
        {
            switch (commandsParameters[0].ToLower())
            {
                case "open":
                    return DoCommand(Command.Open, commandsParameters);
                case "exit":
                    return DoCommand(Command.Exit, commandsParameters);
                case "cls":
                    return DoCommand(Command.Clear, commandsParameters);
                case "help":
                    return DoCommand(Command.Help, commandsParameters);
                default:
                    return false;
            }
        }
        private bool DoCommand(IConsoleCommand command, List<string> commandArgs)
        {
            return command.ExecuteCommand(commandArgs);
        }        
        private List<string> SplitInput() // разделяем строку ввода на команды и аргументы подряд
        {
            string currentInput = Input;
            var splitedInput = currentInput.Split(' ');
            var lineNumber = splitedInput.Length;
            var spaceCounter = 0;
            List<string> noSpacesList = splitedInput.ToList();
            for (int i = 0; i < lineNumber; i++)
            {
                if (splitedInput[i] == "")
                {
                    noSpacesList.RemoveAt(i - spaceCounter);
                    spaceCounter += 1;
                }

            }
            return noSpacesList;
        }

        
    }
}