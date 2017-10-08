using System;
using System.Collections.Generic;
using DatabaseLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml.Commands;

namespace TestsForTree.Commands
{
    [TestClass()]
    public class OpenDbCommandTests
    {
        [TestMethod()]
        public void ExecuteHelpCommandTest()
        {
            ConsoleCommand command = new OpenDbCommand();
            var help = command.ExecuteHelpCommand();
            Assert.AreEqual("Opendb \t This command allow you to open a database\n", help);
        }

        [TestMethod()]
        public void ExecuteOpenDbCommand()
        {
            ConnectionData connectionData = new ConnectionData();
            connectionData.CreateConnectionString("default", null, null);
            OpenDbCommand command = new OpenDbCommand();
            command.ConnectionData = connectionData;
            List<string> commandArgs = new List<string>() { "opendb", "pubs" };
            var result = command.ExecuteCommand(commandArgs);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void ExecuteOpenDbCommandFalse()
        {
            ConnectionData connectionData = new ConnectionData();
            connectionData.CreateConnectionString("default", null, null);
            OpenDbCommand command = new OpenDbCommand();
            command.ConnectionData = connectionData;
            List<string> commandArgs = new List<string>() { "opendb", "pubs2" };
            var result = command.ExecuteCommand(commandArgs);
            Assert.AreEqual(true, result);
        }
    }
}