using System.Collections.Generic;
using TreeXml.Commands;
using TreeXmlLibrary;

namespace TestsForTree
{
    public class TestHelper
    {
        public static Node TestNode()//тестовое дерево
        {
            Node rootData = new Node("Employees");
            rootData.AddAttribute("Emp1", "Denys1");
            Node rootCh1 = new Node("Employee1");
            rootCh1.AddAttribute("Emp2", "Denys2");
            Node rootCh2 = new Node("Employee2");
            rootCh2.AddAttribute("Emp3", "Denys3");
            rootCh2.AddAttribute("Emp4", "Denys4");
            Node rootCh3 = new Node("Employee3");
            rootCh3.AddAttribute("Emp23", "Denys23");
            rootCh3.AddAttribute("Emp114", "Denys1234");
            Node rootCh4 = new Node("Employee4");
            rootCh4.AddAttribute("Emp2", "Denys2");
            rootCh4.Value = "asd";
            Node rootCh5 = new Node("Employee5");
            rootCh5.AddAttribute("as", "asd");
            rootCh5.Value = "as";


            var ch1 = rootData.AddChild(rootCh1);
            var ch2 = rootData.AddChild(rootCh2);

            var ch3 = ch2.AddChild(rootCh3);
            var ch5 = ch3.AddChild(rootCh5);
            var ch4 = ch2.AddChild(rootCh4);

            return rootData;
        }

        public static bool CommandExecute(ConsoleCommand command, List<string> inputString)
        {
            return  command.ExecuteCommand(inputString);
        }
    }
}