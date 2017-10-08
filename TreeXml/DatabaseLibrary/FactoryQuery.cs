using System;
using DatabaseLibrary.DatabaseNodes;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Script;
using TreeXmlLibrary;

namespace DatabaseLibrary
{
    public class FactoryQuery
    {

        public DataBaseItem SelectDbItem(Node node) // выбор нужного класса для создания нужного элемента
        {
            var groupItem = node.Name;
            switch (groupItem)
            {
                case SingleItem.Server:
                    return new ServerReader();
                case GroupItem.Databases:
                    return new DbReader();
                case GroupItem.Schemas:
                    return new SchemaReader();
                case GroupItem.UserTypes:
                    return new TypeReader();
                case GroupItem.Tables:
                    return new TableReader();
                case GroupItem.Views:
                    return new ViewReader();
                case GroupItem.Columns:
                    return new ColumnReader();
                case GroupItem.Keys:
                    return new KeysReader();
                case GroupItem.Constraints:
                    return new ConstraintReader();
                case GroupItem.Procedures:
                    return new ProcedureReader();
                case GroupItem.Triggers:
                    return new TriggerReader();
                case GroupItem.Indexes:
                    return new IndexReader();
                case GroupItem.Parameters:
                    return new ParameterReader();
                case GroupItem.Functions:
                    return new FunctionReader();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DbItemScriptBase CreateScriptItem(Node node)// выбор нужного класса для создания скрипта
        {
            if (node == null)
                return null;
            var script = node.Name;
            switch (script)
            {
                case SingleItem.Database:
                    return new DatabaseScript(node);
                case SingleItem.Schema:
                    return new SchemaScript(node);
                case SingleItem.UserType:
                    return new TypeScript(node);
                case SingleItem.DefaultConstraint:
                case SingleItem.CheckConstraint:
                    return new ConstraintScript(node);
                case SingleItem.Index:
                    return new IndexScript(node);
                case SingleItem.PrimaryKey:
                case SingleItem.ForeignKey:
                    return new KeyScript(node);
                case SingleItem.View:
                case SingleItem.Function:
                case SingleItem.Procedure:
                    return new ViewProcFuncScript(node);
                case SingleItem.Trigger:
                    return new TriggerScript(node);
                case SingleItem.Table:
                    return new TableScript(node);
                default:
                    return null;
            }
        }

        public IPrinter GetPrinter(Node node)
        {
            if (node == null)
                return null;
            var script = node.Name;
            switch (script)
            {
                case SingleItem.Database:
                case GroupItem.Databases:
                    return new DatabaseScript(node);
                case SingleItem.Schema:
                case GroupItem.Schemas:
                    return new SchemaScript(node);
                case SingleItem.UserType:
                case GroupItem.UserTypes:
                    return new TypeScript(node);
                case SingleItem.DefaultConstraint:
                case SingleItem.CheckConstraint:
                case GroupItem.Constraints:
                    return new ConstraintScript(node);
                case SingleItem.Index:
                case GroupItem.Indexes:
                    return new IndexScript(node);
                case SingleItem.PrimaryKey:
                case SingleItem.ForeignKey:
                case GroupItem.Keys:
                    return new KeyScript(node);
                case SingleItem.View:
                case SingleItem.Function:
                case SingleItem.Procedure:
                case GroupItem.Views:
                case GroupItem.Procedures:
                case GroupItem.Functions:
                    return new ViewProcFuncScript(node);
                case SingleItem.Trigger:
                case GroupItem.Triggers:
                    return new TriggerScript(node);
                case SingleItem.Table:
                case GroupItem.Tables:
                    return new TableScript(node);
                default:
                    return null;
            }
        }
    }
}