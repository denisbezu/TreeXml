using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml;
using TreeXml.Helpers;

namespace TestsForTree.Helpers
{
    [TestClass()]
    public class OpenDbCmdCheckerTests
    {

        [TestMethod()]
        public void CheckArgumentTest()
        {
            CmdChecker checker = new OpenDbCmdChecker();
            Parameter parameter = new Parameter("opendb", "pubs");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CheckArgumentTestFalse()
        {
            CmdChecker checker = new OpenDbCmdChecker();
            Parameter parameter = new Parameter("opendb", "");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void CheckCommandTest()
        {
            CmdChecker checker = new OpenDbCmdChecker();
            Parameter parameter = new Parameter("opendb", "pubs");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CheckCommandTestFalse()
        {
            CmdChecker checker = new OpenDbCmdChecker();
            Parameter parameter = new Parameter("asdasda", "default");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }


    }
}