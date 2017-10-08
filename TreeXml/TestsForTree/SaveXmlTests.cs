using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;

namespace TestsForTree
{
    [TestClass()]
    public class SaveXmlTests
    {
        [TestMethod()]
        public void CreateXmlTestFileExist()
        {
            SaveXml saveXml = new SaveXml();
            string path = "testFile.xml";
            var root = TestHelper.TestNode();
            Assert.IsInstanceOfType(root, typeof(Node));
            saveXml.Save(root, path);
            Assert.IsTrue(File.Exists(path));
        }

    }
}