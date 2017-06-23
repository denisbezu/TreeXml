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
                if (SearchPicker(currentNode.Instance, employee))
                {
                    step = counter;
                    return currentNode;
                }
                else
                {
                    nodesQueue.Dequeue();
                    foreach (var children in currentNode.Children)
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
                if (SearchPicker(currentNode.Instance, employee))
                {
                    step = counter;
                    return currentNode;
                }
                foreach (var children in currentNode.Children)
                {
                    nodesStack.Push(children);
                }
            }
            step = counter;
            return null;
        }

        private bool SearchById(Employee currentEmployee, Employee searchableEmployee)
        {
            if (currentEmployee.Id.Equals(searchableEmployee.Id))
                return true;
            return false;
        }

        private bool SearchByNameLastName(Employee currentEmployee, Employee searchableEmployee)
        {
            if (currentEmployee.LastName.Equals(searchableEmployee.LastName)
                && currentEmployee.Name.Equals(searchableEmployee.Name))
                return true;
            return false;
        }

        private bool SearchByAll(Employee currentEmployee, Employee searchableEmployee)
        {
            if (currentEmployee.Equals(searchableEmployee))
                return true;
            return false;
        }

        private bool SearchPicker(Employee currentEmployee, Employee searchableEmployee)
        {
            if (searchableEmployee.Id != 0 && searchableEmployee.LastName != null
                && searchableEmployee.Name != null && searchableEmployee.Age != 0
                && searchableEmployee.Position != null)
                return SearchByAll(currentEmployee, searchableEmployee);
            else if (searchableEmployee.Id != 0 && searchableEmployee.LastName == null
                     && searchableEmployee.Name == null && searchableEmployee.Age == 0
                     && searchableEmployee.Position == null)
                return SearchById(currentEmployee, searchableEmployee);
            else if (searchableEmployee.Id == 0 && searchableEmployee.LastName != null
                     && searchableEmployee.Name != null && searchableEmployee.Age == 0
                     && searchableEmployee.Position == null)
                return SearchByNameLastName(currentEmployee, searchableEmployee);
            return false;// возможно не так и потребуетсяя добавить и другие варианты
        }
    }
}
