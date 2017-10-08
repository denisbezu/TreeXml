using System;

namespace TreeXml
{
    public class Parameter : IEquatable<Parameter>, ICloneable
    {
        private string _argument;
        public string Name { get; set; }

        public string Argument
        {
            get => _argument;
            set
            {
                if (value == null) return;
                _argument = value;
            }
        }
        
        public Parameter(string name, string argument)
        {
            Name = name;
            Argument = argument;
        }
        
        #region Equals
        public bool Equals(Parameter other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_argument, other._argument) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Parameter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_argument != null ? _argument.GetHashCode() : 0) * 397)
                       ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion

        public object Clone()
        {
            return new Parameter(string.Copy(Name), string.Copy(Argument));
        }

        
    }
}