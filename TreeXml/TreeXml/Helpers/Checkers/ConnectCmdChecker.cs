using System;
using System.Collections.Generic;

namespace TreeXml
{
    public class ConnectCmdChecker : CmdChecker
    {
        protected override void InitializeProperties()
        {
            Commands = new Dictionary<string, Predicate<string>>
            {
                ["connect"] = CheckConnectCommand,
                ["-l"] = CheckConnectCommand,
                ["-p"] = CheckConnectCommand
            };
        }

        private bool CheckConnectCommand(string argument)
        {
            if (!string.IsNullOrEmpty(argument))
                return true;
            throw new Exception("The argument of connect command is empty, please try again");
        }

        public override bool CheckArgument(Parameter parameter)
        {
            return Commands[parameter.Name](parameter.Argument);
        }

        public override bool CheckCommand(IList<Parameter> parameters, Parameter parameter)
        {
            if (parameters.Contains(parameter))
                throw new Exception("The parameter " + parameter.Name + " with value " + parameter.Argument + " is already added!Try again");
            return true;
        }

    }
}