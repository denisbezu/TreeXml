using System;
using System.Collections.Generic;

namespace TreeXml
{
    public abstract class CmdChecker
    {
        protected Dictionary<string, Predicate<string>> Commands { get; set; }

        public CmdChecker()
        {
            InitializeProperties();
        }

        protected abstract void InitializeProperties();

        public virtual bool CheckArgument(Parameter parameter)
        {
            return Commands[parameter.Name](parameter.Argument);
        }

        public abstract bool CheckCommand(IList<Parameter> parameters, Parameter parameter);
    }
}