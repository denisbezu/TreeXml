using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TreeXml.Checkers;

namespace TreeXml.Helpers
{
    public class OpenDbCmdChecker : OpenCmdChecker
    {
        protected override void InitializeProperties()
        {
            Commands = new Dictionary<string, Predicate<string>>()
            {
                ["-mode"] = CheckModeCommand,
                ["-a"] = CheckAlgoCommand,
                ["-type"] = CheckTypeCommand,
                ["opendb"] = CheckAddStringCommand, // может быть нужно проверять подключение к БД
                ["-showtree"] = CheckShowCommand,
                ["-showscript"] = CheckShowCommand,
                ["-outxml"] = CheckOutputCommand,
                ["-outscript"] = CheckOutputScriptCommand,
            };
            CommandParams = new[] { "-a", "-outxml", "opendb", "-mode", "-type", "-showtree", "-showscript", "-outscript" };
        }

        private bool CheckOutputScriptCommand(string argument)//проверка команды вывода
        {
            Regex regex = new Regex(@"^.+\.sql$");
            if (!regex.IsMatch(argument))
                throw new Exception("The argument of output command is not correct, please try again");
            return true;
        }
    }
}