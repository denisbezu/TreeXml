using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TreeXmlLibrary;

namespace TreeXml
{
    public class ConsoleApi
    {
        private string Input { get; set; }
        public Node<Employee> Root { get; set; }
        public ConsoleApi()
        {

        }
        public void StartInput()
        {
            bool inputContinue = true;
            while (inputContinue)
            {
                Input = Console.ReadLine();
                var commandsParameters = SplitInput();
                if (commandsParameters.Count == 1)
                {
                    SingleCommandExecution(ref inputContinue); // выполняем комманды help, cls, exit
                }
                else
                {
                    if (CorrectArgCommands(commandsParameters))
                        DoCommands(commandsParameters);
                    else
                    {
                        Console.Write("Invalid command, retry please");
                    }
                }
                Console.WriteLine();
            }
        }

        private void SingleCommandExecution(ref bool inputContinue)
        {
            switch (Input.Trim().ToLower())
            {
                case "help":
                    HelpCommand();
                    break;
                case "exit":
                    inputContinue = false;
                    break;
                case "cls":
                    Console.Clear();
                    break;
                default:
                    Console.Write("Invalid command, retry please");
                    break;
            }
        }

        private void DoCommands(List<string> commandsArgs)
        {
            for (int i = 0; i < commandsArgs.Count; i += 2)
            {
                switch (commandsArgs[i].ToLower())
                {
                    case "cls":
                    case "help":
                    case "exit":
                        SingleHelpCommand(commandsArgs[i]);
                        break;
                    case "open":

                        break;
                }
            }





            //foreach (var pair in CommandArgument)
            //{
            //    switch (pair.Key)
            //    {
            //        case "-s":
            //            ShowTree(pair.Value);
            //            break;
            //        case "-a":
            //            SearchTestCommand(pair.Value);
            //            break;
            //    }
            //}
        }


        private void SingleHelpCommand(string command) // возможно следует переделать все на возращаемое значение string
        {
            switch (command.ToLower())
            {
                case "cls":
                    Console.Write("CLS \t This command allows you to clear the screen");
                    break;
                case "help":
                    Console.Write("HELP \t This command allows you to view all available commands");
                    break;
                case "exit":
                    Console.Write("EXIT \t This command allows you to exit the program");
                    break;
            }
        }
        private void HelpCommand()
        {
            Console.WriteLine("Available commands: ");
            Console.Write("-s \t Show the tree \nhelp \t Show this help \nexit \t" +
                          " close program\ncls \t clear console\n-a \t Search algorithm");
        }
        
        private bool CorrectArgCommands(List<string> commandsArgs) // проверяем все аргументы
        {
            bool allArgumentsCorrect = true;
            switch (commandsArgs[0].ToLower())
            {
                case "cls":
                case "help":
                case "exit":
                    if (commandsArgs.Count != 2 || !CheckSingleCommand(commandsArgs[1]))
                        return false;
                    break;
                case "open":

                    break;
                default:
                    allArgumentsCorrect = false;
                    break;
            }
            if (commandsArgs.Count % 2 == 1)
                allArgumentsCorrect = false;
            return allArgumentsCorrect;
        }

        private bool CheckSingleCommand(string parameter)
        {
            if (parameter == "/?")
                return true;
            return false;
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
            Employee empSearch = new Employee { Id = 7 };
            int step;
            Node<Employee> searcherResult;
            if (argument.ToLower().Equals("level"))
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

        private void ShowTree(string argument)
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
    }
}