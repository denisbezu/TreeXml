using System;
using System.Collections.Generic;
using DatabaseLibrary.Enums;
using TreeXml.Enum;

namespace TreeXml.Commands
{
    public abstract class ConsoleCommand
    {
        protected CmdChecker Checker { get; set; }

        private bool CheckCommand(string parameter)//проверка правильности ввода команды помощи
        {
            if (parameter == "/?")
                return true;
            return false;
        }

        public bool ExecuteCommand(List<string> commandArgs)//выполнение команды
        {
            //try
            //{
                if (commandArgs.Count == 2 && CheckCommand(commandArgs[1]))
                {
                    Console.Write(ExecuteHelpCommand());
                    return true;
                }
                return ExecuteSpecialCommand(commandArgs);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    //Console.WriteLine(e.StackTrace);
            //    return true;
            //}
        }

        public abstract string ExecuteHelpCommand();
        protected abstract bool ExecuteSpecialCommand(List<string> commandArgs);


        protected bool CreateParameters(IList<string> commandArgs)
        {
            var parameters = new List<Parameter>();
            for (int i = 0; i < commandArgs.Count; i++)
            {
                Parameter parameter = new Parameter(commandArgs[i], commandArgs[i + 1]);
                if (CheckParameter(parameters, parameter))
                    parameters.Add(parameter);
                i++;
            }
            return ExecuteParameters(parameters);
        }

        protected virtual bool ExecuteParameters(IList<Parameter> parameters)
        {
            return true;
        }

        private bool CheckParameter(IList<Parameter> parameters, Parameter parameter)
        {
            return Checker.CheckCommand(parameters, parameter) && Checker.CheckArgument(parameter);
        }
    }
}