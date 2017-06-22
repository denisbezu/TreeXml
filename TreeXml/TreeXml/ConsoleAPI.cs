using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TreeXmlLibrary;

namespace TreeXml
{
    public class ConsoleApi
    {
        public string Input { get; set; }
        public Dictionary<string, string> CommandArgument { get; set; }
        public Node<Employee> Root { get; set; }

        public ConsoleApi()
        {
            CommandArgument = new Dictionary<string, string>();
        }

        public void StartInput()
        {
            bool inputContinue = true;
            while (inputContinue)
            {
                Input = Console.ReadLine();
                if (SplitInput().Count == 1)
                {
                    switch (Input)
                    {
                        case "help":
                            Help();
                            break;
                        case "exit":
                            inputContinue = false;
                            break;
                        case "-s":
                            ShowTree("test");
                            break;
                        case "cls":
                            Console.Clear();
                            break;
                        default:
                            Console.Write("Invalid command, retry please");
                            break;
                    }
                }
                else
                {
                    DictionaryFill(SplitInput());
                    if (CorrectArgCommands())
                        DoWork();
                    else
                    {
                        Console.Write("Invalid command, retry please");
                    }
                }
                CommandArgument.Clear();
                Console.WriteLine();
            }
        }

        public void DoWork()
        {
            foreach (var pair in CommandArgument)
            {
                switch (pair.Key)
                {
                    case "-s":
                        ShowTree(pair.Value);
                        break;
                }
            }
        }

        public void Help()
        {
            Console.WriteLine("Available commands: ");
            Console.Write("-s \t Show the tree \nhelp \t Show this help \nexit \t" +
                          " close program\ncls \t clear console");
        }

        public void ShowTree(string argument)
        {
            if (argument == "test")
                Console.Write(Root);
            else
            {
                Console.Write("Openning file " + argument + "...");
            }
        }

        private bool CorrectArgCommands() // проверяем все аргументы
        {
            bool allArgumentsCorrect = true;
            foreach (var pair in CommandArgument)
            {
                switch (pair.Key)
                {
                    case "-s":
                        if (!CheckShowCommand(pair.Key, pair.Value))
                            return false;
                        break;
                    default:
                        allArgumentsCorrect = false;
                        break;
                }
            }
            if (CommandArgument.Count * 2 != SplitInput().Count)
                allArgumentsCorrect = false;

            if (!allArgumentsCorrect)
            {
                //Console.Write("Invalid command, retry please");
                return false;
            }
            return true;
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

        private void DictionaryFill(List<string> splitInput)
        {
            if (splitInput.Count % 2 != 0)
                return;
            for (int i = 0; i < splitInput.Count; i += 2)
            {
                if (!CommandArgument.ContainsKey(splitInput[i]))
                    CommandArgument.Add(splitInput[i], splitInput[i + 1]);
            }
        } // заполняем словарь команда-аргумент

        private bool CheckShowCommand(string command, string argument) // проверка команды отображения дерева
        {
            Regex regex = new Regex(@"^[a-z0-9]+\.xml$");
            if (command.Equals("-s") && (argument.Equals("test") || regex.IsMatch(argument)))
                return true;
            return false;
        }
    }
}