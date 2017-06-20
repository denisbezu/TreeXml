using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeXmlLibrary;

namespace TreeXml
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee emp1 = new Employee(1, "Denys", "Bezuhlyi", 20);
            Employee emp2 = new Employee(2, "Valeriy", "Pugachov", 21);
            Employee emp3 = new Employee(1, "Petya", "Vasev", 20);
            emp2.AddEmployee(emp1);
            emp2.AddEmployee(emp3);
            foreach (var emp in emp2.Subworkers)
            {
                Console.WriteLine(emp.LastName);
            }

        }
    }
}
