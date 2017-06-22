using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeXmlLibrary
{
    public class Searcher
    {
        public Node<Employee> WidthSearchFirst(Node<Employee> rootNode, Employee employee, out int step)
        {
            var counter = 0;

            var nodesQueue = new Queue<Node<Employee>>();
            nodesQueue.Enqueue(rootNode);
            while (nodesQueue.Count != 0)
            {
                counter++;
                var currentNode = nodesQueue.Peek();
                if (currentNode.Temp.Equals(employee))
                {
                    step = counter;
                    return currentNode;
                }
                else
                {
                    nodesQueue.Dequeue();
                    foreach (var children in currentNode.Childrens)
                    {
                        nodesQueue.Enqueue(children);
                    }
                }
            }
            step = counter;
            return null;
        }

        public Node<Employee> LevelSearchFirst(Node<Employee> rootNode, Employee employee, out int step)
        {
            var counter = 0;
            var nodesStack = new Stack<Node<Employee>>();
            nodesStack.Push(rootNode);
            while (nodesStack.Count != 0)
            {
                counter++;
                var currentNode = nodesStack.Pop();
                if (currentNode.Temp.Equals(employee))
                {
                    step = counter;
                    return currentNode;
                }
                foreach (var children in currentNode.Childrens)
                {
                    nodesStack.Push(children);
                }
            }
            step = counter;
            return null;
        }
    }
}
