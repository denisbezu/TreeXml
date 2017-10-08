using System;
using System.Collections.Generic;
using System.Linq;
using TreeXml.Checkers;
using TreeXml.Enum;
using TreeXmlLibrary;
using TreeXmlLibrary.Enums;
using TreeXmlLibrary.interfaces;
using TreeXmlLibrary.Search;

namespace TreeXml.Commands
{
    public class OpenCommand : ConsoleCommand
    {
        //текущее дерево
        protected Tree Tree { get; set; }

        protected Searcher Searcher { get; set; }

        public OpenCommand()
        {
            Checker = new OpenCmdChecker();
        }

        public override string ExecuteHelpCommand() //помощь
        {
            return "Open \t This command allows you to open the file \n" +
                   "Usage: open [filename] [-[parameter] value]\n" +
                   "Available parameters and values:\nout\t\t Output file(value - filename)\n" +
                   "a \t\t Search algorithm (value - level or width)\n" +
                   "text \t\t Search by text (value - value of node)\n" +
                   "name \t\t Search by name (value - name of node)\n" +
                   "hasvalue \t\t Search by Attribute name (value - name of attribute)\n" +
                   "show \t\t Allows to show the tree (value - y or n)\n" +
                   "att[attribute name] \t\t Search by attribute (value - value of attribute)\n" +
                   "mode \t\t Allows you to specify a mode of search (value - first or all)\n" +
                   "type \t\t Allows you to specify a type of research (value - or, and, names)";
        }

        protected override bool ExecuteSpecialCommand(List<string> commandArgs)
        {
            Searcher = new Searcher();
            if (commandArgs.Count % 2 == 0)
                return CreateParameters(commandArgs);
            return false;
        }

        #region Search
        protected virtual List<Parameter> ResearchParameters(IList<Parameter> parameters)//отсеивание параметров поиска
        {
            List<Parameter> searchParams = new List<Parameter>();
            foreach (var parameter in parameters)
            {
                if (parameter.Name.ToLower() != OpenAttribute.Open &&
                    parameter.Name.ToLower() != OpenAttribute.Show &&
                    parameter.Name.ToLower() != OpenAttribute.Output)
                {
                    var searchParameter = parameter.Clone() as Parameter;
                    searchParameter.Name = parameter.Name.Substring(1);
                    searchParams.Add(searchParameter);
                }
            }
            return searchParams;
        }

        protected List<Node> AlgCommand(List<Parameter> searchParameters)//метод для вызова поиска 
        {
            var searchResult = SearchCommand(searchParameters);
            if (searchResult != null)
            {
                foreach (var node in searchResult)
                    DrawSearchResult(node);
                DrawResult(Searcher.Step, Searcher.Runtime);
                return searchResult;
            }
            Console.WriteLine(@"Nothing found");
            return null;
        }

        private void SetSearchParameters(List<Parameter> parameters)
        {
            Searcher.Step = 0;
            Searcher.Runtime = "";
            foreach (var parameter in parameters)
            {
                switch (parameter.Name.ToLower())
                {
                    case "a":
                        {
                            Searcher.AlgoType = parameter.Argument.ToLower().Equals("level") ? AlgoType.Level : AlgoType.Width;
                            break;
                        }
                    case "name":
                        {
                            Searcher.Names.Add(parameter.Argument);
                            break;
                        }
                    case "text":
                        {
                            Searcher.Textes.Add(parameter.Argument);
                            break;
                        }
                    case "hasvalue":
                        {
                            Searcher.Attributes.Add(new TreeXmlLibrary.Attribute() { Name = parameter.Argument });
                            break;
                        }
                    case "type":
                        {
                            if (parameter.Argument.ToLower().Equals("and"))
                                Searcher.SearchType = SearchType.And;
                            else
                                Searcher.SearchType = SearchType.Names;
                            break;
                        }
                    case "mode":
                        {
                            Searcher.SearchMode = parameter.Argument.ToLower().Equals("first") ? SearchMode.First : SearchMode.All;
                            break;
                        }
                    default:
                        {
                            Searcher.Attributes.Add(new TreeXmlLibrary.Attribute(parameter.Name.Substring(3), parameter.Argument));
                            break;
                        }
                }
            }
        }

        private List<Node> SearchCommand(List<Parameter> searchParameters)//поиск узла/узлов
        {
            if (Tree == null)
                return null;
            SetSearchParameters(searchParameters);
            return Searcher.Search(Tree.Root);
        }

        #endregion

        #region Console Output

        protected void OutputXmlCommand(string outputPath, List<Node> searchResult)// метод для вывода в файл
        {
            if (searchResult == null)
            {
                SaveTreeXml(outputPath, Tree.Root);
                return;
            }
            if (searchResult.Count == 1)
                SaveTreeXml(outputPath, searchResult.First());
            else
            {
                var root = TreeRootFormer(searchResult);
                SaveTreeXml(outputPath, root);
            }
        }

        private void DrawResult(int step, string runtime)// печать времени и шагов
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("Number of steps: {0}\n-----------------", step);
            Console.WriteLine("Runtime : " + runtime + " milliseconds");
        }

        private void DrawSearchResult(Node node)// печать узлов
        {
            Console.WriteLine("Found node:\nName : " + node.Name);
            if (node.Value != null)
                Console.WriteLine("Value : " + node.Value);
            if (node.Attributes != null)
            {
                Console.WriteLine("Attributes:");
                foreach (var attribute in node.Attributes)
                {
                    Console.WriteLine(attribute.Name + " : " + attribute.Value);
                }
            }
            Console.WriteLine("-----------------");
            ShowTree(node);
        }

        protected void ShowTree(Node node)//отображение дерева начиная с заданного узла
        {
            if (node != null)
            {
                var consoleDrawer = new NodeXmlString();
                var tree = consoleDrawer.DrawTree(node);
                Console.WriteLine(tree);
            }
            else
                throw new Exception("The node item isnt set, so it is not possible to show the tree");
        }


        #endregion

        #region SaveOpen

        private void OpenXmlFile(string parameter) // команда открытия файла
        {
            IOpenFile openXml = new OpenXml();
            Tree = openXml.Open(parameter);
            Console.WriteLine("The file " + parameter + " is openned");
        }

        private void SaveTreeXml(string argument, Node node)// вывод в файл
        {
            if (node != null)
            {
                ISaveFile saveXml = new SaveXml();
                saveXml.Save(node, argument);
                Console.WriteLine("Extracting current tree to the file " + argument + "...");
            }
            else
                throw new Exception("The root item isnt set, so you cannot use the output command. Cannot save your tree to the file");
        }

        protected Node TreeRootFormer(IList<Node> nodes, string rootName = "root")//формирование дерева для сохранения нескольких найденных результатов
        {
            Node root = new Node(rootName);
            Tree tree = new Tree(root);
            foreach (var node in nodes)
            {
                Node child = (Node)node.Clone();
                child.RemoveParent();
                root.AddChild(child);
            }
            return root;
        }
        #endregion

        protected bool CheckSearchableData(List<Parameter> searchParameters)// проверка, задан ли хотя бы один параметр поиска
        {
            if (searchParameters.GetByName("name") == null && searchParameters.GetByName("text") == null
                && searchParameters.CheckStartsWith("att") == null && searchParameters.GetByName("hasvalue") == null)
                throw new Exception("You most to set at least one paremeter of research, please try again");
            return true;
        }

        protected override bool ExecuteParameters(IList<Parameter> parameters)
        {
            List<Node> searchResult = null;
            var searchParameters = ResearchParameters(parameters);
            if (parameters.GetByName(OpenAttribute.Algorithm) != null && !CheckSearchableData(searchParameters))
                return false;
            if (parameters.GetByName(OpenAttribute.Open) != null)
                OpenXmlFile(parameters.GetByName(OpenAttribute.Open).Argument);
            if (parameters.GetByName(OpenAttribute.Show) != null 
                && parameters.GetByName(OpenAttribute.Show).Argument.ToLower().Equals("y"))
                ShowTree(Tree.Root);
            if (parameters.GetByName(OpenAttribute.Algorithm) != null)
                searchResult = AlgCommand(searchParameters);
            if (parameters.GetByName(OpenAttribute.Output) != null)
                OutputXmlCommand(parameters.GetByName(OpenAttribute.Output).Argument, searchResult);
            return true;
        }
        
    }

}
