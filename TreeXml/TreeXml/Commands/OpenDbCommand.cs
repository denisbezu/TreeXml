using System;
using System.Collections.Generic;
using DatabaseLibrary;
using DatabaseLibrary.Enums;
using TreeXml.Enum;
using TreeXml.Helpers;
using TreeXmlLibrary;

namespace TreeXml.Commands
{
    public class OpenDbCommand : OpenCommand
    {
        public ConnectionData ConnectionData { get; set; }

        public OpenDbCommand()
        {
            Checker = new OpenDbCmdChecker();
        }

        public override string ExecuteHelpCommand()
        {
            return "Opendb \t This command allow you to open a database\n";
        }

        protected override bool ExecuteSpecialCommand(List<string> commandArgs)//выполнение команды
        {
            if (ConnectionData == null)
                throw new Exception("You are not connected to the server");
            return base.ExecuteSpecialCommand(commandArgs);
        }

        protected override bool ExecuteParameters(IList<Parameter> parameters)//выполнение заданных команд и аргументов
        {
            List<Node> searchResult = null;
            var searchParameters = ResearchParameters(parameters);
            if (parameters.GetByName(OpenAttribute.Algorithm) != null && !CheckSearchableData(searchParameters))
                return false;
            if (parameters.GetByName(OpenAttribute.OpenDb) != null)
                OpenDb(parameters.GetByName(OpenAttribute.OpenDb).Argument);
            if (parameters.GetByName(OpenAttribute.ShowTree) != null &&
                parameters.GetByName(OpenAttribute.ShowTree).Argument.ToLower().Equals("y"))
                ShowTree(Tree.Root);
            if (parameters.GetByName(OpenAttribute.Algorithm) != null)
                searchResult = AlgCommand(searchParameters);
            if (parameters.GetByName(OpenAttribute.ShowScript) != null &&
                parameters.GetByName(OpenAttribute.ShowScript).Argument.ToLower().Equals("y"))
                ShowScript(searchResult);
            if (parameters.GetByName(OpenAttribute.OutXml) != null)
                OutputXmlCommand(parameters.GetByName(OpenAttribute.OutXml).Argument, searchResult);
            if (parameters.GetByName(OpenAttribute.OutScript) != null)
                OutputScriptCommand(parameters.GetByName(OpenAttribute.OutScript).Argument, searchResult);
            return true;
        }

        protected override List<Parameter> ResearchParameters(IList<Parameter> parameters)// отсеивание параметров поиска
        {
            var searchParams = new List<Parameter>();
            foreach (var parameter in parameters)
            {
                if (parameter.Name.ToLower() != OpenAttribute.OpenDb &&
                    parameter.Name.ToLower() != OpenAttribute.ShowScript &&
                    parameter.Name.ToLower() != OpenAttribute.ShowTree &&
                    parameter.Name.ToLower() != OpenAttribute.OutXml &&
                    parameter.Name.ToLower() != OpenAttribute.OutScript)
                {
                    var searchParameter = parameter.Clone() as Parameter;
                    if (searchParameter != null)
                    {
                        searchParameter.Name = parameter.Name.Substring(1);
                        searchParams.Add(searchParameter);
                    }
                }
            }
            return searchParams;
        }

        private void ShowScript(List<Node> searchResult)//отображение скрипта в консоли
        {
            var factoryQuery = new FactoryQuery();
            if (searchResult == null)
            {
                Console.WriteLine(factoryQuery.CreateScriptItem(Tree.Root.GetChildByName(SingleItem.Database)).GenerateScript());
            }
        }

        private void OutputScriptCommand(string argument, List<Node> searchResult) //вывод скрипта в файл
        {
            SaveScript saver = new SaveScript();
            saver.Save(argument, searchResult, Tree);
            Console.WriteLine($@"Script created ({argument})");
        }

        private void OpenDb(string argument) //открытие бд
        {
            DatabaseReader databaseReader = new DatabaseReader();
            databaseReader.ConnectionData = ConnectionData;
            Console.WriteLine(argument + @" openned!");
            Tree = new Tree(databaseReader.DatabaseCreator(argument));
            Console.WriteLine(argument + @" loaded!");
        }
    }
}