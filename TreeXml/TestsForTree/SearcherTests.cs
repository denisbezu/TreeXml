using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;
using TreeXmlLibrary.Enums;
using TreeXmlLibrary.Search;

namespace TestsForTree
{
    [TestClass()]
    public class SearcherTests
    {
        [TestMethod()]
        public void WidthSearchAllTest()
        {
            var searcher = new Searcher()
            {
                AlgoType = AlgoType.Width,
                Names = { "Employee4" },
                SearchMode = SearchMode.All
            };
            int neededStep = 6;
            searcher.Search(TestHelper.TestNode());
            Assert.AreEqual(neededStep, searcher.Step);
        }
        [TestMethod()]
        public void LevelSearchAllTest()
        {
            var searcher = new Searcher()
            {
                AlgoType = AlgoType.Level,
                Names = { "Employee4" },
                SearchMode = SearchMode.All
            };
            int neededStep = 6;
            searcher.Search(TestHelper.TestNode());
            Assert.AreEqual(neededStep, searcher.Step);
        }
        [TestMethod()]
        public void WidthSearchFirstTest()
        {
            var searcher = new Searcher()
            {
                AlgoType = AlgoType.Width,
                Names = { "Employee4" },
                SearchMode = SearchMode.First
            };
            int neededStep = 5;
            searcher.Search(TestHelper.TestNode());
            Assert.AreEqual(neededStep, searcher.Step);
        }
        [TestMethod()]
        public void LevelSearchFirstTest()
        {
            var searcher = new Searcher()
            {
                AlgoType = AlgoType.Level,
                Names = { "Employee4" },
                SearchMode = SearchMode.First
            };
            int neededStep = 3;
            var first = searcher.Search(TestHelper.TestNode());
            Assert.AreEqual(neededStep, searcher.Step);
        }


    }
}