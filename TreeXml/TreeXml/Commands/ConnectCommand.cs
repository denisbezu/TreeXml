using System;
using System.Collections.Generic;
using DatabaseLibrary;
using DatabaseLibrary.Enums;
using TreeXml.Enum;

namespace TreeXml.Commands
{
    public class ConnectCommand : ConsoleCommand
    {
        public ConnectionData ConnectionData { get; set; }

        public ConnectCommand()
        {
            Checker = new ConnectCmdChecker();
        }

        public override string ExecuteHelpCommand()
        {
            return "CONNECT \t This command allows you to connect to the database\n";
        }

        protected override bool ExecuteSpecialCommand(List<string> commandArgs)
        {
            if (commandArgs.Count % 2 == 0)
            {
                ConnectionData = new ConnectionData();
                return CreateParameters(commandArgs);
            }
            return false;
        }
        
        protected override bool ExecuteParameters(IList<Parameter> parameters)
        {
            string server = parameters.GetByName(ConnectAttribute.Connect)?.Argument;
            string login = parameters.GetByName(ConnectAttribute.Login)?.Argument;
            string pass = parameters.GetByName(ConnectAttribute.Password)?.Argument;
            if (parameters.Count > 1)
                ConnectionData.CreateConnectionString(server, login, pass);
            else
                ConnectionData.CreateConnectionString(server);
            ConnectionData.CheckServerConnection();
            Console.WriteLine(@"Connected to " + ConnectionData.ServerName);
            return true;
        }
    }
}