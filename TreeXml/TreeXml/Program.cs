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
            ConsoleApi consoleApi = new ConsoleApi();
            consoleApi.Root = TestNode();
            consoleApi.StartInput();
        }
        static Node<Employee> TestNode()
        {
            Employee emp1 = new Employee(1, "Denys", "Bezuhlyi", 20, "Project Manager");
            Employee emp2 = new Employee(2, "Kostya", "Petrov", 21, "Project Manager");
            Employee emp3 = new Employee(1, "Petya", "Vasev", 20, "Project Manager");
            Employee emp4 = new Employee(7, "Kolya", "Petrov", 25, "HR");
            Employee emp5 = new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager");
            Employee emp6 = new Employee(1, "Not", "Needed", 25, "QA Engineer");
            Employee emp7 = new Employee(3, "Doha", "Den", 26, "Project Manager");
            Tree<Employee> tree = new Tree<Employee>();

            var root = tree.AddRoot(emp2);
            try
            {
                var c1 = tree.AddNode(emp3, root); // +
                var c2 = tree.AddNode(emp1, root); // +
                var c4 = tree.AddNode(emp5, c1); // +
                var c123 = tree.AddNode(emp4, root); // +
                var c5 = tree.AddNode(emp3, c4); // -
                //var c6 = c5.AddNode(emp4); // -
                var c10 = tree.AddNode(emp6, c4); // +
                // var c12 = c4.AddNode(emp7); // +
                var c145 = tree.AddNode(emp7, c123); // -
                //var c7 = c6.AddNode(emp2); // -
                //var c8 = c6.AddNode(emp3); // -
                Node<Employee> c9;
                if (c5 != null)
                    c9 = tree.AddNode(emp2, c5); // -
               // var c35 = c145.AddNode(emp2);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
            Tree<Employee> tree2 = new Tree<Employee>();
            //Employee employee = new Employee();
            //employee.LastName = "Denys";
            //Console.WriteLine(employee);


            var root2 = tree2.AddRoot(emp3);
            return root;
        }

    }

}
