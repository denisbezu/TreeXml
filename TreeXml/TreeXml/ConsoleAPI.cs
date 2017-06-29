using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeXml.Interfaces;

namespace TreeXml
{
    public class ConsoleApi
    {
        private string Input { get; set; }
        public void StartInput()
        {
            while (true)
            {
                Console.Write(Directory.GetCurrentDirectory() + ">");
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
            noSpacesList = CheckMergeQuot(noSpacesList);
            return noSpacesList;
        }

        private List<string> CheckMergeQuot(List<string> splittedList)// если есть ввод с двойными кавычками, то объединяем их
        {
            int rowNumber = splittedList.Count;
            string resultString = "";
            bool mergeContinue = false;
            int insertIndex = 0, countModification = 0;
            for (int i = 0; i < rowNumber; i++)
            {
                if (splittedList[i].StartsWith("\""))
                {
                    mergeContinue = true;
                    insertIndex = i;
                }
                if (splittedList[i].EndsWith("\""))
                {
                    resultString += splittedList[i];
                    mergeContinue = false;
                    i -= countModification;
                    rowNumber -= countModification;
                    splittedList[insertIndex] = resultString.Replace('"', ' ').Trim();
                    splittedList.RemoveRange(insertIndex + 1, countModification);
                    countModification = 0;
                    resultString = "";
                }
                if (mergeContinue)
                {
                    countModification++;
                    resultString += splittedList[i] + " ";
                }
            }
            return splittedList;
        }
    }
}