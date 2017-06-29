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
            ClearCommand clearCommand = new ClearCommand();
            List<string> inputSplit = new List<string> { "cls" };
            bool expected = true;
            bool actual = clearCommand.ExecuteCommand(inputSplit);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidSingleCommand()
        {
            ClearCommand clearCommand = new ClearCommand();
            List<string> inputSplit = new List<string> { "cls " };
            bool expected = false;
            bool actual = clearCommand.ExecuteCommand(inputSplit);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestHelpCommand()
        {
            ClearCommand clearCommand = new ClearCommand();
            List<string> inputSplit = new List<string> { "cls", "/?" };
            bool expected = true;
            bool actual = clearCommand.ExecuteCommand(inputSplit);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidHelp()
        {
            ClearCommand clearCommand = new ClearCommand();
            List<string> inputSplit = new List<string> { "cls", "/?", "cls", "/?" };
            bool expected = false;
            bool actual = clearCommand.ExecuteCommand(inputSplit);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandTestInValidCommand()
        {
            ClearCommand clearCommand = new ClearCommand();
            List<string> inputSplit = new List<string> { "help" };
            bool expected = false;
            bool actual = clearCommand.ExecuteCommand(inputSplit);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExecuteHelpCommandTestValid()
        {
            string expected = "CLS \t This command allows you to clear the screen\n";
            ClearCommand command = new ClearCommand();
            string actual = command.ExecuteHelpCommand();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteHelpCommandTestInValid()
        {
            string expected = "CLS \t This command allows you to clear the screen\n      ";
            ClearCommand command = new ClearCommand();
            string actual = command.ExecuteHelpCommand();
            Assert.AreNotEqual(expected, actual);
        }
        
    }
}