using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TreeXml.Interfaces;
using TreeXmlLibrary;

namespace TreeXml.Commands
{
    public class OpenCommand : IConsoleCommand
    {
        public Node<Employee> Root { get; set; }
        public bool ExecuteCommand(List<string> commandArgs)
        {
            if (commandArgs.Count == 2 && commandArgs[1].ToLower().Equals("/?"))
            {
                Console.Write(ExecuteHelpCommand());
                return true;
            }
            else if (commandArgs.Count % 2 == 0)
            {
                return CheckOpenCommands(commandArgs);

            }

            return false;
        }
        public Employee SearchableEmployee { get; set; }

        public string ExecuteHelpCommand()
        {
            return "Open \t This command allows you to open the file \n" +
                   "Usage: open [file ] [-[parameter] value]\n" +
                   "Available values and parameters:\nout\t\t Output file\n" +
                   "a \t\t Search algorithm (level or width)\n" +
                   "id \t\t Search by id (value - id)\n" +
                   "name \t\t Search by name (value - name)\n" +
                   "lastname \t Search by lastname (value - lastname)\n" +
                   "age \t\t Search by age (value - age)\n" +
                   "position \t Search by position (value - position)\n" +
                   "show \t\t Allows to show the tree (value - y or n)\n" +
                   "You can find the needed item by using search by id, by lastname and name or by all attributes\n";
        }

        private bool CheckOpenCommands(List<string> commandArgs)
        {
            Dictionary<string, bool> bools = new Dictionary<string, bool>
            {
                {"open", false }, {"output", false }, {"alg", false}, {"id", false}, {"name", false},
                { "lastname", false}, {"age", false}, {"position", false}, {"show", false}
            };
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "open", null }, { "output", null }, { "alg", null }, { "show", null },
            };
            for (int i = 0; i < commandArgs.Count; i += 2)
            {
                switch (commandArgs[i].ToLower())
                {
                    case "open":
                        {
                            if (CheckOpenCommand(commandArgs[i + 1]) && bools["open"] == false)
                            {
                                parameters["open"] = commandArgs[i + 1];
                                bools["open"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-show":
                        {
                            if (CheckShowCommand(commandArgs[i + 1]) && bools["show"] == false)
                            {
                                parameters["show"] = commandArgs[i + 1];
                                bools["show"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-out":
                        {
                            if (CheckOutputCommand(commandArgs[i + 1]) && bools["output"] == false)
                            {
                                parameters["output"] = commandArgs[i + 1];
                                bools["output"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-a":
                        {
                            if (CheckAlgoCommand(commandArgs[i + 1]) && bools["alg"] == false)
                            {
                                SearchableEmployee = new Employee();
                                parameters["alg"] = commandArgs[i + 1];
                                bools["alg"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-id":
                        {
                            int idValue;
                            if (CheckAddIntCommand(commandArgs[i + 1], bools["alg"], out idValue) && bools["id"] == false)
                            {
                                SearchableEmployee.Id = idValue;
                                bools["id"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-name":
                        {
                            if (CheckAddStringCommand(commandArgs[i + 1], bools["alg"]) && bools["name"] == false)
                            {
                                SearchableEmployee.Name = commandArgs[i + 1];
                                bools["name"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-lastname":
                        {
                            if (CheckAddStringCommand(commandArgs[i + 1], bools["alg"]) && bools["lastname"] == false)
                            {
                                SearchableEmployee.LastName = commandArgs[i + 1];
                                bools["lastname"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-age":
                        {
                            int ageValue;
                            if (CheckAddIntCommand(commandArgs[i + 1], bools["alg"], out ageValue) && bools["age"] == false)
                            {
                                SearchableEmployee.Age = ageValue;
                                bools["age"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    case "-position":
                        {
                            if (CheckAddStringCommand(commandArgs[i + 1], bools["alg"]) && bools["position"] == false)
                            {
                                SearchableEmployee.Position = commandArgs[i + 1];
                                bools["position"] = true;
                            }
                            else
                                return false;
                            break;
                        }
                    default:
                        return false;
                }
            }
            return DoAllCommands(bools, parameters);
        }

        private bool DoAllCommands(Dictionary<string, bool> bools, Dictionary<string, string> parameters)
        {
            if (bools["alg"] && !CheckSearchableEmployee())
                return false;
            foreach (var pair in bools)
            {
                if (pair.Value)
                {
                    switch (pair.Key)
                    {
                        case "open":
                            OpenFile(parameters["open"]);
                            break;
                        case "show":
                            if (parameters["show"].ToLower().Equals("y"))
                                ShowTree(parameters["open"]);
                            break;
                        case "output":
                            ExtractTree(parameters["output"]);
                            break;
                        case "alg":
                            if (bools["alg"] && CheckSearchableEmployee())
                                SearchCommand(parameters["alg"], SearchableEmployee);
                            break;
                    }
                }
            }
            return true;
        }
        
        private bool OpenFile(string parameter) // поправить
        {
            Console.WriteLine("I'am trying to open the file...");
            if (parameter.ToLower().Equals("test"))
            {
                Root = TestNode();
                return true;
            }
            else
            {
                Saver saver = new Saver();
                Root = saver.LoadXml(parameter);
                return true;
            }
            return false;

        }

        private void SearchCommand(string argument, Employee searchableEmployee)
        {
            Searcher searcher = new Searcher();
            int step;
            Node<Employee> searcherResult;
            if (argument.ToLower().Equals("level"))
                searcherResult = searcher.LevelSearchFirst(Root, searchableEmployee, out step);
            else
                searcherResult = searcher.WidthSearchFirst(Root, searchableEmployee, out step);

            if (searcherResult != null)
            {
                Console.Write("----------------\nFound node:\nID : " + searcherResult.Value.Id + "\nName : " + searcherResult.Value.Name + "\n" +
                              "Lastname : " + searcherResult.Value.LastName + "\nAge : " +
                              searcherResult.Value.Age + "\nPosition : " + searcherResult.Value.Position + "\n");
                Console.WriteLine("Number of steps: {0}\n-----------------", step);
            }
            else
            {
                Console.WriteLine("Nothing found");
            }
        }

        private void ShowTree(string argument)
        {
            //добавить потом проверку на null root-у
            if (Root != null)
            {
                var consoleDrawer = new ConsoleDrawer();
                var tree = consoleDrawer.DrawTree(Root);
                Console.Write(tree);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("I am showing the file " + argument + "...");
            }
        }

        private void ExtractTree(string argument)// сделать вывод в файл еще
        {
            Console.WriteLine("Extracting tree to the file " + argument + "...");
        }

        #region Checkers
        private bool CheckOpenCommand(string argument) // проверка команды открытия дерева
        {
            Regex regex = new Regex(@"^[a-z0-9]+\.xml$");
            if (argument.Equals("test") || regex.IsMatch(argument))
                return true;
            return false;
        }
        private bool CheckAlgoCommand(string argument)
        {
            if (argument.ToLower().Equals("level") || argument.ToLower().Equals("width"))
                return true;
            return false;
        }
        private bool CheckSearchableEmployee()// изменить
        {
            if (SearchableEmployee.Id != 0 && SearchableEmployee.Age != 0 && SearchableEmployee.Name != null &&
                SearchableEmployee.LastName != null && SearchableEmployee.Position != null)
                return true;
            else if (SearchableEmployee.Id != 0)
                return true;
            else if ( SearchableEmployee.Name != null && SearchableEmployee.LastName != null )
                return true;
            else
                return false;
        }
        private bool CheckShowCommand(string parameter)
        {
            if (parameter.ToLower().Equals("y") || parameter.ToLower().Equals("n"))
                return true;
            return false;
        }
        private bool CheckOutputCommand(string argument)
        {
            Regex regex = new Regex(@"^[a-z0-9]+\.xml$");
            if (regex.IsMatch(argument))
                return true;
            return false;
        }
        private bool CheckAddIntCommand(string argument, bool algValue, out int number)
        {
            bool result = Int32.TryParse(argument, out number);
            if (result && algValue && number > 0)
            {
                return true;
            }
            return false;
        }
        private bool CheckAddStringCommand(string argument, bool boolValue)
        {
            if (boolValue && !string.IsNullOrEmpty(argument))
            {
                return true;
            }
            return false;
        }
        #endregion

        static Node<Employee> TestNode()
        {
            Employee emp1 = new Employee(1, "Denys", "Bezuhlyi", 20, "Project Manager");
            Employee emp2 = new Employee(2, "Kostya", "Petrov", 21, "Project Manager");
            Employee emp3 = new Employee(1, "Petya", "Vasev", 20, "Project Manager");
            Employee emp4 = new Employee(7, "Kolya", "Petrov", 25, "HR");
            Employee emp5 = new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager");
            Employee emp6 = new Employee(1, "Not", "Needed", 25, "QA Engineer");
            Employee emp7 = new Employee(3, "Doha", "Den", 26, "Project Manager");
            Tree<Employee> tree = new Tree<Employee>();
            
            var root = tree.AddRoot(emp2);
            try
            {
                var c1 = tree.AddNode(emp3, root); // +
                var c2 = tree.AddNode(emp1, root); // +
                var c4 = tree.AddNode(emp5, c1); // +
                var c123 = tree.AddNode(emp4, root); // +
                var c5 = tree.AddNode(emp3, c4); // -
                //var c6 = c5.AddNode(emp4); // -
                var c10 = tree.AddNode(emp6, c4); // +
                // var c12 = c4.AddNode(emp7); // +
                var c145 = tree.AddNode(emp7, c123); // -
                //var c7 = c6.AddNode(emp2); // -
                //var c8 = c6.AddNode(emp3); // -
                Node<Employee> c9;
                if (c5 != null)
                    c9 = tree.AddNode(emp2, c5); // -
                // var c35 = c145.AddNode(emp2);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
            Tree<Employee> tree2 = new Tree<Employee>();
            //Employee employee = new Employee();
            //employee.LastName = "Denys";
            //Console.WriteLine(employee);


            var root2 = tree2.AddRoot(emp3);
            return root;
        }
    }
}