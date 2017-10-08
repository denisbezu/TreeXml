using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXml;

namespace TestsForTree.Helpers
{
    [TestClass()]
    public class ConnectCmdCheckerTests
    {
        [TestMethod()]
        public void CheckArgumentTest()
        {
            CmdChecker checker = new ConnectCmdChecker();
            Parameter parameter = new Parameter("connect", "default");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }
        
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CheckArgumentTestFalse()
        {
            CmdChecker checker = new ConnectCmdChecker();
            Parameter parameter = new Parameter("connect", "");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void CheckCommandTest()
        {
            CmdChecker checker = new ConnectCmdChecker();
            Parameter parameter = new Parameter("connect", "default");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CheckCommandTestFalse()
        {
            CmdChecker checker = new ConnectCmdChecker();
            Parameter parameter = new Parameter("asdasda", "default");
            var result = checker.CheckArgument(parameter);
            Assert.AreEqual(true, result);
        }
    }
}