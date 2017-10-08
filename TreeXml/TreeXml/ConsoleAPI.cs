using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DatabaseLibrary;
using TreeXml.Commands;
using TreeXml.Enum;

namespace TreeXml
{
    public class ConsoleApi
    {
        private string Input { get; set; } // введенная строка

        private ConnectionData ConnectionData { get; set; }

        public void StartInput()//ввод 
        {
            while (true)
            {
                Console.Write(Directory.GetCurrentDirectory() + @">");
                Input = Console.ReadLine();
                var commandsParameters = SplitInput();
                if (commandsParameters == null)
                {
                    InValidCommand();
                    Console.WriteLine();
                }
                else if (commandsParameters.Count != 0)
                    if (!CheckInputCommand(commandsParameters))
                    {
                        InValidCommand();
                        Console.WriteLine();
                    }
            }
        }

        private void InValidCommand()//ошибка ввода
        {
            Console.Write(@"Invalid command, retry please");
        }

        private bool CheckInputCommand(List<string> commandsParameters)//создаем экземляр нужного класса
        {
            ConsoleFactory consoleFactory = new ConsoleFactory();
            switch (commandsParameters[0].ToLower())
            {
                case "open":
                    return consoleFactory.GetConsoleCommand(ECommand.Open).ExecuteCommand(commandsParameters);
                case "exit":
                    return consoleFactory.GetConsoleCommand(ECommand.Exit).ExecuteCommand(commandsParameters);
                case "cls":
                    return consoleFactory.GetConsoleCommand(ECommand.Clear).ExecuteCommand(commandsParameters);
                case "help":
                    return consoleFactory.GetConsoleCommand(ECommand.Help).ExecuteCommand(commandsParameters);
                case "connect":
                        return ConnectCmd(consoleFactory, commandsParameters);
                case "opendb":
                    {
                        var openDbCmd = (OpenDbCommand)consoleFactory.GetConsoleCommand(ECommand.OpenDb);
                        openDbCmd.ConnectionData = ConnectionData;
                        return openDbCmd.ExecuteCommand(commandsParameters);
                    }
                default:
                    return false;
            }
        }

        private List<string> SplitInput()
        {
            string currentInput = Input;
            List<string> argsCommands = new List<string>();
            string word = @"(""[^""]+"")|\S+";
            foreach (Match match in Regex.Matches(currentInput, word))
            {
                var foundWord = match.Value.Replace("\"", "");
                argsCommands.Add(foundWord);
            }
            return argsCommands;
        }

        private bool ConnectCmd(ConsoleFactory consoleFactory, List<string> commandsParameters)
        {
            var consoleCmd = (ConnectCommand)consoleFactory.GetConsoleCommand(ECommand.Connect);
            var commandOk = consoleCmd.ExecuteCommand(commandsParameters);
            if (commandOk)
                ConnectionData = consoleCmd.ConnectionData;
            return commandOk;
        }
    }
}