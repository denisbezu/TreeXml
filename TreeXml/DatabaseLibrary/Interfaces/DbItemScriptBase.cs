using System;
using System.Linq;
using System.Text;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary.Interfaces
{
    public abstract class DbItemScriptBase : IPrinter
    {
        protected Node CurrentNode { get; set; }

        protected string ItemName { get; set; }

        public DbItemScriptBase(Node node)
        {
            CurrentNode = node;
        }

        public string GenerateScript()
        {
            if (CurrentNode == null)
                throw new Exception("Current node cannot be null!");
            string script = GetScript();
            if (!string.IsNullOrEmpty(script))
            {
                if (script.EndsWith(Go()))
                    return script;
                return script + Go();
            }
            return string.Empty;
        }

        public string GetUseDb()
        {
            return $"USE [{CurrentNode.FindAncestor(SingleItem.Database)?.GetAttributeValue(ObjectAttribute.Name)}]{Go()}";
        }

        protected abstract string GetScript();

        protected string Go()
        {
            return "\nGO\n\n";
        }

        public string Print()
        {
            SetNames();
            if (SingleItem.IsSingleItem(ItemName))
                return GenerateScript();
            if (GroupItem.IsGroupItem(ItemName))
                return GenerateGroupScript(CurrentNode);
            throw new ArgumentException("Invalid node name");
        }

        protected abstract void SetNames(); 

        private string GenerateGroupScript(Node node)
        {
            var factoryQuery = new FactoryQuery();
            var builder = new StringBuilder();
            foreach (var nodeChild in node.Children)
            {
                var script = factoryQuery.CreateScriptItem(nodeChild);
                builder.Append(script.GenerateScript());
            }
            return builder.ToString();
        }

        protected bool CheckParametersForNull(params string[] parameters)
        {
            return parameters.All(parameter => parameter != null);
        }
    }
}