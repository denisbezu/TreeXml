using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;

namespace TestsForTree
{
    [TestClass()]
    public class OpenXmlTests
    {
        [TestMethod()]
        public void LoadXmlTest()
        {
            string path = "testFile.xml";
            OpenXml openXml = new OpenXml();
            bool errorChecker = true;
            string errorMessage = "";
            var tree = openXml.LoadXml<Employee>(path, out errorChecker, out errorMessage);
            Assert.IsNotNull(tree);
            Assert.IsTrue(errorChecker);
            Assert.AreEqual("", errorMessage);
        }
    }
}