using System;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class ViewProcFuncScript : DbItemScriptBase
    {

        public ViewProcFuncScript(Node node) : base(node)
        {

        }

        protected override string GetScript()
        {
            if (CheckParametersForNull(CurrentNode.GetAttributeValue(ObjectAttribute.Definition)))
                return CurrentNode.GetAttributeValue(ObjectAttribute.Definition);
            return string.Empty;
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.Function:
                case SingleItem.View:
                case SingleItem.Procedure:
                case GroupItem.Functions:
                case GroupItem.Procedures:
                case GroupItem.Views:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }
    }
}