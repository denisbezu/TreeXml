using DatabaseLibrary;
using DatabaseLibrary.DatabaseNodes;
using DatabaseLibrary.Script;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;

namespace TestsForTree
{
    [TestClass()]
    public class FactoryQueryTests
    {
        [TestMethod()]
        public void SelectDbItemTest()
        {
            FactoryQuery factoryQuery = new FactoryQuery();
            Node node = new Node("Tables");
            var result = factoryQuery.SelectDbItem(node);
            Assert.IsInstanceOfType(result, typeof(TableReader));
        }

        [TestMethod()]
        public void SelectScriptTest()
        {
            FactoryQuery factoryQuery = new FactoryQuery();
            Node node = new Node("Table");
            var result = factoryQuery.CreateScriptItem(node);
            Assert.IsInstanceOfType(result, typeof(TableScript));
        }
    }
}