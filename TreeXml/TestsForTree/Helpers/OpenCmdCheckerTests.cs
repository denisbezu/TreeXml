using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml;
using TreeXml.Checkers;
using TreeXml.Helpers;

namespace TestsForTree.Helpers
{
    [TestClass()]
    public class OpenCmdCheckerTests
    {
        [TestMethod()]
        public void CheckArgumentTest()
        {
            CmdChecker checker = new OpenCmdChecker();
            Parameter parameter = new Parameter("open", "books.xml");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CheckArgumentTestFalse()
        {
            CmdChecker checker = new OpenCmdChecker();
            Parameter parameter = new Parameter("open", "");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void CheckCommandTest()
        {
            CmdChecker checker = new OpenCmdChecker();
            Parameter parameter = new Parameter("open", "books.xml");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CheckCommandTestFalse()
        {
            CmdChecker checker = new OpenCmdChecker();
            Parameter parameter = new Parameter("asdasda", "default");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }
    }
}