using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml.Commands;

namespace TestsForTree.Commands
{
    [TestClass]
    public class HelpCommandTest
    {
        [TestMethod()]
        public void ExecuteCommandTestValidSingleCommand()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new HelpCommand(), new List<string>() { "help" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidSingleCommand()
        {
            bool expected = false;
            bool actual = TestHelper.CommandExecute(new HelpCommand(), new List<string>() { "help " });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestHelpCommand()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new HelpCommand(), new List<string>() { "help", "/?" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidHelp()
        {
            bool expected = false;
            bool actual = TestHelper.CommandExecute(new HelpCommand(), new List<string>() { "help", "/?", "help", "/?" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidCommand()
        {
            bool expected = false;
            bool actual = TestHelper.CommandExecute(new HelpCommand(), new List<string>() { "cls" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteHelpCommandTestValid()
        {
            string expected = "HELP \t This command allows you to view all available commands\n";
            ConsoleCommand command = new HelpCommand();
            string actual = command.ExecuteHelpCommand();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteHelpCommandTestInValid()
        {
            string expected = "HELP \t This command allows you to view all available commands\n      ";
            ConsoleCommand command = new HelpCommand();
            string actual = command.ExecuteHelpCommand();
            Assert.AreNotEqual(expected, actual);
        }
    }
}