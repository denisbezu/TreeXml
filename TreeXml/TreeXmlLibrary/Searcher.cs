using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace TreeXmlLibrary
{
    public class Searcher
    {
        public Node<T> WidthSearchFirst<T>(Node<T> rootNode, T t, out int step, out string runtime) where T : class
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var counter = 0;
            var nodesQueue = new Queue<Node<T>>();
            nodesQueue.Enqueue(rootNode);
            while (nodesQueue.Count != 0)
            {
                counter++;
                var currentNode = nodesQueue.Peek();
                if (SearchPicker(currentNode.Value, t))
                {
                    step = counter;
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    runtime = ts.TotalMilliseconds.ToString();
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
            runtime = "";
            return null;
        }

        public Node<T> LevelSearchFirst<T>(Node<T> rootNode, T t, out int step, out string runtime) where T : class
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var counter = 0;
            var nodesStack = new Stack<Node<T>>();
            nodesStack.Push(rootNode);
            while (nodesStack.Count != 0)
            {
                counter++;
                var currentNode = nodesStack.Pop();
                if (SearchPicker(currentNode.Value, t))
                {
                    step = counter;
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    runtime = ts.TotalMilliseconds.ToString();
                    return currentNode;
                }
                foreach (var children in currentNode.Children)
                {
                    nodesStack.Push(children);
                }
            }
            step = counter;
            runtime = "";
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

        private bool SearchByAll<T>(T currentInstance, T searchableInstance) where T : class
        {
            if (currentInstance.Equals(searchableInstance))
                return true;
            return false;
        }

        private bool SearchPicker<T>(T currentInstance, T searchableInstance) where T : class
        {
            if (currentInstance is Employee && searchableInstance is Employee)
            {
                Employee currentEmployee = currentInstance as Employee;
                Employee searchableEmployee = searchableInstance as Employee;
                //if (searchableEmployee.Id != 0 && searchableEmployee.LastName == null
                //    && searchableEmployee.Name == null && searchableEmployee.Age == 0
                //    && searchableEmployee.Position == null)
                //    return SearchById(currentEmployee, searchableEmployee);
                //else if (searchableEmployee.Id == 0 && searchableEmployee.LastName != null
                //         && searchableEmployee.Name != null && searchableEmployee.Age == 0
                //         && searchableEmployee.Position == null)
                //    return SearchByNameLastName(currentEmployee, searchableEmployee);
                //else if (searchableEmployee.Id != 0 && searchableEmployee.LastName != null
                //         && searchableEmployee.Name != null && searchableEmployee.Age != 0
                //         && searchableEmployee.Position != null)
                //    return SearchByAll(currentInstance, searchableInstance);
                if (searchableEmployee.Id != 0 && searchableEmployee.Age != 0 && searchableEmployee.Name != null &&
                searchableEmployee.LastName != null && searchableEmployee.Position != null)
                    return SearchByAll(currentEmployee, searchableEmployee);
                else if (searchableEmployee.Id != 0)
                    return SearchById(currentEmployee, searchableEmployee);
                else if (searchableEmployee.Name != null && searchableEmployee.LastName != null)
                    return SearchByNameLastName(currentEmployee, searchableEmployee);
            }
            else
                return SearchByAll(currentInstance, searchableInstance);

            return false;// возможно не так и потребуетсяя добавить и другие варианты
        }
    }
}
