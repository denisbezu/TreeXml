using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsForTree
{
    [TestClass()]
    public class ConsoleDrawerTests
    {
        [TestMethod()]
        public void DrawTreeTest()
        {
            ConsoleDrawer consoleDrawer = new ConsoleDrawer();
            var root = TestHelper.TestNode();
            string tree = consoleDrawer.DrawTree(root);

            string consoleTree =
                "<Employees>" +
                "\n\t<Employee Id=\"1\" Name=\"Petya\" LastName=\"Vasev\" Age=\"20\" Position=\"Project Manager\" >" +
                "\n\t\t<Employee Id=\"1\" Name=\"Sasha\" LastName=\"Maslenikov\" Age=\"25\" Position=\"Project Manager\" >" +
                "\n\t\t\t<Employee Id=\"1\" Name=\"Not\" LastName=\"Needed\" Age=\"25\" Position=\"QA Engineer\" />" +
                "\n\t\t</Employee>" +
                "\n\t</Employee>" +
                "\n\t<Employee Id=\"1\" Name=\"Denys\" LastName=\"Bezuhlyi\" Age=\"20\" Position=\"Project Manager\" />" +
                "\n\t<Employee Id=\"7\" Name=\"Kolya\" LastName=\"Petrov\" Age=\"25\" Position=\"HR\" >" +
                "\n\t\t<Employee Id=\"3\" Name=\"Doha\" LastName=\"Den\" Age=\"26\" Position=\"Project Manager\" />" +
                "\n\t</Employee>" +
                "\n</Employees>\n";
            Assert.AreEqual(consoleTree, tree);
        }
    }
}