using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml.Commands;

namespace TestsForTree.Commands
{
    [TestClass()]
    public class ExitCommandTests
    {
        [TestMethod()]
        public void ExecuteCommandTest()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new ExitCommand(), new List<string>() { "exit", "/?" });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ExecuteHelpCommandTest()
        {
            ConsoleCommand command = new ExitCommand();
            string actual = command.ExecuteHelpCommand();
            string needed = "EXIT \t This command allows you to exit the program\n";
            Assert.AreEqual(needed, actual);
        }
    }
}