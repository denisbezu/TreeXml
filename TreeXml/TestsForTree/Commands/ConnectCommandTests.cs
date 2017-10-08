using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml.Commands;

namespace TestsForTree.Commands
{
    [TestClass()]
    public class ConnectCommandTests
    {
        [TestMethod()]
        public void ExecuteHelpCommandTest()
        {
            ConsoleCommand command = new ConnectCommand();
            var help = command.ExecuteHelpCommand();
            Assert.AreEqual("CONNECT \t This command allows you to connect to the database\n", help);
        }

        [TestMethod()]
        public void ExecuteConnectCommand()
        {
            ConsoleCommand command = new ConnectCommand();
            List<string> commandArgs = new List<string>() { "connect", "default"};
            command.ExecuteCommand(commandArgs);
            Assert.AreEqual(true, true);
        }


    }
}