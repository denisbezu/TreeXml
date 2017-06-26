using System.Collections.Generic;

namespace TreeXml.Interfaces
{
    public interface IConsoleCommand
    {
        bool ExecuteCommand(List<string> commandArgs);
        string ExecuteHelpCommand();
        //bool CheckCommand(string parameter);
    }
}