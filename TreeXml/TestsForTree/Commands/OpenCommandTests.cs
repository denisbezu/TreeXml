using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml.Commands;

namespace TestsForTree.Commands
{
    [TestClass()]
    public class OpenCommandTests
    {
        [TestMethod()]
        public void ExecuteCommandTest()
        {

            bool expected = false;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open" });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ExecuteHelpCommandTest()
        {
            ConsoleCommand command = new OpenCommand();
            List<string> splittedInput = new List<string>() { "open", "/?" };
            string actual = command.ExecuteHelpCommand();
            string needed = "Open \t This command allows you to open the file \n" +
                            "Usage: open [filename] [-[parameter] value]\n" +
                            "Available parameters and values:\nout\t\t Output file(value - filename)\n" +
                            "a \t\t Search algorithm (value - level or width)\n" +
                            "text \t\t Search by text (value - value of node)\n" +
                            "name \t\t Search by name (value - name of node)\n" +
                            "hasvalue \t\t Search by Attribute name (value - name of attribute)\n" +
                            "show \t\t Allows to show the tree (value - y or n)\n" +
                            "att[attribute name] \t\t Search by attribute (value - value of attribute)\n" +
                            "mode \t\t Allows you to specify a mode of search (value - first or all)\n" +
                            "type \t\t Allows you to specify a type of research (value - or, and, names)";

            Assert.AreEqual(needed, actual);
        }

        [TestMethod]
        public void ExecuteCommandOpenValidFile()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open", "testfile.xml" });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExecuteCommandOpenInValidFile()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open", "testfileInValid.xml" });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExecuteCommandOutput()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open", "testfile.xml", "-out", "denys.xml" });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExecuteCommandOutputInValid()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open", "testfileInValid.xml", "-out", "denys.xml" });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExecuteCommandShowTest()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open", "testfileInValid.xml", "-show", "y" });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExecuteCommandShowInValidTest()
        {
            bool expected = true;
            bool actual = TestHelper.CommandExecute(new OpenCommand(), new List<string>() { "open", "testfileInValid.xml", "-show23", "y" });
            Assert.AreEqual(expected, actual);
        }
    }
}