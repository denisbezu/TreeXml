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

          
            //consoleApi.Input = "-s /?    help  faf  fdfndsf  asdasd ";
            // consoleApi.DictionaryFill(consoleApi.SplitInput());
        }

        static Node<Employee> TestNode()
        {
            Employee emp1 = new Employee(1, "Denys", "Bezuhlyi", 20, "Project Manager");
            Employee emp2 = new Employee(2, "Valeriy", "Pugachov", 21, "Project Manager");
            Employee emp3 = new Employee(1, "Petya", "Vasev", 20, "Project Manager");
            Employee emp4 = new Employee(1, "Kolya", "Petrov", 25, "HR");
            Employee emp5 = new Employee(1, "Sasha", "Maslenikov", 25, "Project Manager");
            Employee emp6 = new Employee(1, "Not", "Needed", 25, "QA Engineer");
            Employee emp7 = new Employee(3, "Doha", "Den", 26, "Project Manager");
            //Node<Employee> root = new Node<Employee>(emp2);
            //AllNodes<Employee>.Nodes.Add(root.Temp);

            var root = Node<Employee>.AddRoot(emp2);
            try
            {
                var c1 = root.AddNode(emp3); // +
                var c2 = root.AddNode(emp1); // +
                var c4 = c1.AddNode(emp5); // +
                var c123 = root.AddNode(emp4); // +
                var c5 = c4.AddNode(emp3); // -
                //var c6 = c5.AddNode(emp4); // -
                var c10 = c4.AddNode(emp6); // +
                                            // var c12 = c4.AddNode(emp7); // +
                var c145 = c123.AddNode(emp7); // -
                //var c7 = c6.AddNode(emp2); // -
                //var c8 = c6.AddNode(emp3); // -
                Node<Employee> c9;
                if (c5 != null)
                    c9 = c5.AddNode(emp2); // -
               // var c35 = c145.AddNode(emp2);
            }
            catch (Exception e)
            {
            }

            Searcher searcher = new Searcher();
            int step;
            var searcherResult = searcher.LevelSearchFirst(root, emp7, out step);
            if (searcherResult != null)
            {
                Console.Write("Found node:\nName : " + searcherResult.Temp.Name + "\n" +
                              "Lastname : " + searcherResult.Temp.LastName + "\nAge : " +
                              searcherResult.Temp.Age + "\nPosition : " + searcherResult.Temp.Position +"\n");
                Console.WriteLine("Number of steps: {0}", step);
            }
            else
            {
                Console.WriteLine("No");
            }
            return root;
        }

    }

}
