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
            Employee emp4 = new Employee(1, "Kolya", "Petrov", 25);
            Employee emp5 = new Employee(1, "Sasha", "Maslenikov", 25);
            Employee emp6 = new Employee(1, "Not", "Needed", 25);
            var root = new Node<Employee>(emp2);
            AllNodes<Employee>.Nodes.Add(root.Temp);
            var c1 = root.AddNode(emp3);
            var c2 = root.AddNode(emp1);
            var c4 = c1.AddNode(emp5);
            var c123 = root.AddNode(emp4);
            var c5 = c4.AddNode(emp3);
            var c6 = c5.AddNode(emp4);
            var c12 = root.AddNode(emp4);
            var c7 = c6.AddNode(emp2);
            var c8 = c6.AddNode(emp3);
            var c9 = c5.AddNode(emp2);
            var c10 = c5.AddNode(emp6);
            Console.Write(root.ToString());
            Console.WriteLine("\n");
            Console.ReadLine();

            //Console.WriteLine("Removing nodes\n");
            //c2.Children.Remove(c2_2);
            //c2_1.Parent = null;
            //Console.WriteLine(root.ToString());
            //Console.ReadKey();

            Node2<Employee>.ShowEmp(emp2);
        }
    }
    public class Node2<T> where T : class
    {
        public static void ShowEmp(T t)
        {
            Console.WriteLine(t.ToString());
        }
    }
}
