using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeXmlLibrary
{
    public class Employee : IEquatable<Employee>
    { 
        public Employee()
        {
        }

        public Employee(int id, string name, string lastName, int age)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Age = age;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }

        public override string ToString()
        {
            return new StringBuilder().Append(LastName).Append(" ").Append(Name).ToString();
        }


        public bool Equals(Employee secondEmployee)
        {
            if (ReferenceEquals(null, secondEmployee))
                return false;
            if (ReferenceEquals(this, secondEmployee))
                return true;
            return Id == secondEmployee.Id &&
                string.Equals(Name, secondEmployee.Name) &&
                string.Equals(LastName, secondEmployee.LastName) &&
                Age == secondEmployee.Age &&
                string.Equals(Position, secondEmployee.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Employee) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Age;
                hashCode = (hashCode * 397) ^ (Position != null ? Position.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
