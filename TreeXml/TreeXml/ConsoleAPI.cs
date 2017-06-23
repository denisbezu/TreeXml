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
                        DoCommands();
                    else
                    {
                        Console.Write("Invalid command, retry please");
                    }
                }
                CommandArgument.Clear();
                Console.WriteLine();
            }
        }
        public void DoCommands()
        {
            foreach (var pair in CommandArgument)
            {
                switch (pair.Key)
                {
                    case "-s":
                        ShowTree(pair.Value);
                        break;
                    case "-a":
                        SearchTestCommand(pair.Value);
                        break;
                }
            }
        }
        public void Help()
        {
            Console.WriteLine("Available commands: ");
            Console.Write("-s \t Show the tree \nhelp \t Show this help \nexit \t" +
                          " close program\ncls \t clear console\n-a \t Search algorithm");
        }
        public void ShowTree(string argument)
        {
            //добавить потом проверку на null root-у
            if (argument == "test")
            {
                var consoleDrawer = new ConsoleDrawer();
                var tree = consoleDrawer.DrawTree(Root);
                Console.Write(tree);
            }
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
                    case "-a":
                        if (!CheckAlgoCommand(pair.Key, pair.Value))
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
                return false;
            
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
        private void DictionaryFill(List<string> splitInput)// заполняем словарь команда-аргумент
        {
            if (splitInput.Count % 2 != 0)
                return;
            for (int i = 0; i < splitInput.Count; i += 2)
            {
                if (!CommandArgument.ContainsKey(splitInput[i]))
                    CommandArgument.Add(splitInput[i], splitInput[i + 1]);
            }
        } 
        private bool CheckShowCommand(string command, string argument) // проверка команды отображения дерева
        {
            Regex regex = new Regex(@"^[a-z0-9]+\.xml$");
            if (command.Equals("-s") && (argument.Equals("test") || regex.IsMatch(argument)))
                return true;
            return false;
        }
        private bool CheckAlgoCommand(string command, string argument)
        {
            if (command.Equals("-a") && (argument.ToLower().Equals("level") || argument.ToLower().Equals("width")))
                return true;
            return false;
        }
        private void SearchTestCommand(string argument)// добавить emp
        {
            Searcher searcher = new Searcher();
            Employee empSearch = new Employee {Id = 7};
            int step;
            Node<Employee> searcherResult; 
            if(argument.ToLower().Equals("level"))
               searcherResult = searcher.LevelSearchFirst(Root, empSearch/*new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager")*/, out step);
            else
                searcherResult = searcher.WidthSearchFirst(Root, new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager"), out step);
            
            if (searcherResult != null)
            {
                Console.Write("Found node:\nName : " + searcherResult.Instance.Name + "\n" +
                              "Lastname : " + searcherResult.Instance.LastName + "\nAge : " +
                              searcherResult.Instance.Age + "\nPosition : " + searcherResult.Instance.Position + "\n");
                Console.WriteLine("Number of steps: {0}", step);
            }
            else
            {
                Console.WriteLine("No");
            }
        }
    }
}