using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeXmlLibrary;

namespace TestsForTree
{
    [TestClass()]
    public class EmployeeTests
    {
        [TestMethod()]
        public void EqualsEmployeeTest()
        {
            Employee employee1 = new Employee(1, "Denys", "Bezuhlyi", 20, "student");
            Employee employee2 = new Employee(1, "Denys", "Bezuhlyi", 20, "student");
            bool areEqual = employee1.Equals(employee2);
            Assert.AreEqual(areEqual, true);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Employee employee1 = new Employee(1, "Denys", "Bezuhlyi", 20, "student");
            Employee employee2 = new Employee(1, "Denys", "Bezuhlyi", 20, "student");
            Assert.AreEqual(employee1.GetHashCode(), employee2.GetHashCode());
        }
    }
}