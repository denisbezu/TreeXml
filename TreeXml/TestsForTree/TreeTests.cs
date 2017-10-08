using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;

namespace TestsForTree
{
    [TestClass()]
    public class TreeTests
    {
       
        [TestMethod()]
        public void AddNodeTest()
        {
            Node parent = new Node("Employee3");
            parent.AddAttribute("Emp23", "Denys23");
            parent.AddAttribute("Emp114", "Denys1234");
            Node child = new Node("Employee4");
            child.AddAttribute("Emp2", "Denys2");
            parent.AddChild(child);

            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(parent.Children[0], child);
        }
    }
}