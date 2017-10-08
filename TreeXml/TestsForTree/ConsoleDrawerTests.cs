using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml;
using TreeXmlLibrary;


namespace TestsForTree
{
    [TestClass()]
    public class ConsoleDrawerTests
    {
        [TestMethod()]
        public void DrawTreeTest()
        {
            NodeXmlString consoleDrawer = new NodeXmlString(); 
            var root = TestHelper.TestNode();
            string tree = consoleDrawer.DrawTree(root);

            string consoleTree =
                "<Employees Emp1=\"Denys1\">" +
                "\n\t<Employee1 Emp2=\"Denys2\"/>" +
                "\n\t<Employee2 Emp3=\"Denys3\" Emp4=\"Denys4\">" +
                "\n\t\t<Employee3 Emp23=\"Denys23\" Emp114=\"Denys1234\">" +
                "\n\t\t\t<Employee5 as=\"asd\">as</Employee5>" +
                "\n\t\t</Employee3>" +
                "\n\t\t<Employee4 Emp2=\"Denys2\">asd</Employee4>" +
                "\n\t</Employee2>" +
                "\n</Employees>\n";


            Assert.AreEqual(consoleTree, tree);
        }
    }
}