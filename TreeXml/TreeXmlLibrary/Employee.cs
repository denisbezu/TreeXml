using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeXmlLibrary
{
    public class Employee
    {
        public Employee()
        {
            Subworkers = new List<Employee>();
        }

        public Employee(int id, string name, string lastName, int age)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Age = age;
            Subworkers = new List<Employee>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<Employee> Subworkers { get; }

        public void AddEmployee(Employee employee)
        {
            Subworkers.Add(employee);
        }
    }
}
