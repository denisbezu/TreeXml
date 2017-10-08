using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml.Commands;

namespace TestsForTree.Commands
{
    [TestClass()]
    public class ClearCommandTest
    {
        [TestMethod()]
        public void ExecuteCommandTestValidSingleCommand()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new ClearCommand(), new List<string>() { "cls" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestHelpCommand()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new ClearCommand(), new List<string>() { "cls", "/?" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidHelp()
        {
            bool expected = false;
            bool actual = TestHelper.CommandExecute(new ClearCommand(), new List<string>() { "cls", "/?", "cls", "/?" });
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void ExecuteHelpCommandTestValid()
        {
            string expected = "CLS \t This command allows you to clear the screen\n";
            ConsoleCommand command = new ClearCommand();
            string actual = command.ExecuteHelpCommand();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteHelpCommandTestInValid()
        {
            string expected = "CLS \t This command allows you to clear the screen\n      ";
            ConsoleCommand command = new ClearCommand();
            string actual = command.ExecuteHelpCommand();
            Assert.AreNotEqual(expected, actual);
        }
        
    }
}