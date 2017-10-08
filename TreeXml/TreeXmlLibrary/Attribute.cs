using System;

namespace TreeXmlLibrary
{
    public class Attribute : IEquatable<Attribute>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Attribute()
        {
            
        }
        public Attribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public bool Equals(Attribute other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Attribute) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}