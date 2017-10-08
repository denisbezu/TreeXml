using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DatabaseLibrary.Enums;
using TreeXml.Enum;

namespace TreeXml.Checkers
{
    public class OpenCmdChecker : CmdChecker
    {
        protected string[] CommandParams { get; set; }
        private string[] SearchParams { get; set; }

        public OpenCmdChecker()
        {
            SearchParams = new[] { "-name", "-text", "-hasvalue" };
        }

        public override bool CheckCommand(IList<Parameter> parameters, Parameter parameter)
        {
            if (!parameters.Contains(parameter))
            {
                if (CommandParams.Any(p => p == parameter.Name))
                {
                    if (parameters.Any(p => parameter.Name == p.Name))
                        throw new Exception("The parameter " + parameter.Name + " is already added!Try again");
                    return true;
                }
                else
                {
                    if (SearchParams.FirstOrDefault(p => p == parameter.Name) == null && !parameter.Name.StartsWith("-att")) // если параметра нету в списке и не начиенается с -att
                        throw new Exception("Invalid parameter " + parameter.Name + "!Try again.");
                    if (parameters.FirstOrDefault(param => param.Name == "-a") == null) // если еще не было команды алгоритма
                        throw new Exception("You need to use an algorithm command first! Try again.");
                    return true;
                }
            }
            else
                throw new Exception("The parameter " + parameter.Name + " with value " + parameter.Argument + " is already added!Try again");
        }

        public override bool CheckArgument(Parameter parameter)
        {
            if (SearchParams.FirstOrDefault(p => p == parameter.Name) != null ||
                parameter.Name.ToLower().StartsWith("-att"))
                return true;
            return base.CheckArgument(parameter);
        }

        protected override void InitializeProperties()
        {
            Commands = new Dictionary<string, Predicate<string>>
            {
                ["open"] = CheckOpenCommand,
                ["-out"] = CheckOutputCommand,
                ["-show"] = CheckShowCommand,
                ["-mode"] = CheckModeCommand,
                ["-a"] = CheckAlgoCommand,
                ["-type"] = CheckTypeCommand,
            };
            CommandParams = new[] { "-a", "-out", "open", "-mode", "-type", "-show" };
        }

        #region Checkers

        protected bool CheckTypeCommand(string argument)
        {
            if (!argument.ToLower().Equals("and") && !argument.ToLower().Equals("or") && !argument.ToLower().Equals("names"))
                throw new Exception("The argument of type command is not correct, please try again");
            return true;
        }

        private bool CheckOpenCommand(string argument) // проверка команды открытия дерева
        {
            Regex regex = new Regex(@"^.+\.xml$");
            if (!regex.IsMatch(argument))
                throw new Exception("The argument of open command is not correct, please try again");
            return true;
        }

        protected bool CheckModeCommand(string argument)//проверка режима поиска
        {
            if (!argument.ToLower().Equals("all") && !argument.ToLower().Equals("first"))
                throw new Exception("The argument of mode command is not correct, please try again");
            return true;
        }

        protected bool CheckAlgoCommand(string argument)//проверка комманды alg
        {
            if (!argument.ToLower().Equals("level") && !argument.ToLower().Equals("width"))
                throw new Exception("The argument of algorithm command is not correct, please try again");
            return true;
        }

        protected bool CheckShowCommand(string argument)//проверка аргумента команды show
        {
            if (!argument.ToLower().Equals("y") && !argument.ToLower().Equals("n"))
                throw new Exception("The argument of show command is not correct, please try again");
            return true;
        }

        protected bool CheckOutputCommand(string argument)//проверка команды вывода
        {
            Regex regex = new Regex(@"^.+\.xml$");
            if (!regex.IsMatch(argument))
                throw new Exception("The argument of output command is not correct, please try again");
            return true;
        }

        protected bool CheckAddStringCommand(string argument)//проверка строки 
        {
            if (string.IsNullOrEmpty(argument))
                throw new Exception("The argument of search command is empty or the alghorith command is not used, please try again");
            return true;
        }

        #endregion
    }
}