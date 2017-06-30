using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;

namespace TestsForTree
{
    [TestClass()]
    public class SearcherTests
    {
        [TestMethod()]
        public void WidthSearchFirstTest()
        {
            Searcher searcher = new Searcher();
            var root = TestHelper.TestNode();
            Employee emp3 = new Employee(1, "Petya", "Vasev", 20, "Project Manager");
            Employee emp5 = new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager");
            int step, neededStep = 2;
            string runtime = "";
            Node<Employee> parent = new Node<Employee>(emp5);
            Node<Employee> child = new Node<Employee>(emp3) { Parent = parent };
            parent.Children.Add(child);
            var resultSearchNode = searcher.WidthSearchFirst(root, emp3, out step, out runtime);
            Assert.AreEqual(neededStep, step);
            Assert.AreEqual(resultSearchNode, child);
        }

        [TestMethod()]
        public void LevelSearchFirstTest()
        {
            Searcher searcher = new Searcher();
            var root = TestHelper.TestNode();
            Employee emp3 = new Employee(1, "Petya", "Vasev", 20, "Project Manager");
            Employee emp5 = new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager");
            int step, neededStep = 5;
            string runtime = "";
            Node<Employee> parent = new Node<Employee>(emp5);
            Node<Employee> child = new Node<Employee>(emp3) { Parent = parent };
            parent.Children.Add(child);
            var resultSearchNode = searcher.LevelSearchFirst(root, emp3, out step, out runtime);
            Assert.AreEqual(neededStep, step);
            Assert.AreEqual(resultSearchNode, child);
        }
    }
}