using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary.Interfaces
{
    public abstract class DataBaseItem
    {
        protected string NodeName { get; set; }

        protected abstract string GetQuery(params string[] dbItems); // запрос

        public virtual void AddEmptyNodes(Node parentNode) //добавление множественных узлов родителю
        { }

        public virtual void AddChildren(Node parentNode, SqlConnection connection) // добавление узлов родителю, метод по умолчанию
        {
            LoadDbItem(parentNode, GetQuery(ParameterFormer(parentNode)), connection);
            parentNode.SortChildren();
        }

        private void AddEachNode(Node root, DataTable dtQuery) //добавление еденичных узлов родителю
        {
            List<string> attributeNames = GetAttributesList(dtQuery);
            foreach (DataRow row in dtQuery.Rows)
            {
                var node = new Node(NodeName);
                for (int i = 0; i < attributeNames.Count; i++)
                {
                    if (!string.IsNullOrEmpty(row[i].ToString()))
                        node.AddAttribute(attributeNames[i], row[i].ToString());
                }
                root.AddChild(node);
               // AddEmptyNodes(node);
            }
        }

        private List<string> GetAttributesList(DataTable executedQuery) // список названий атрибутов для узла
        {
            var attributes = new List<string>();
            foreach (DataColumn column in executedQuery.Columns)
            {
                attributes.Add(column.ColumnName);
            }
            return attributes;
        }

        protected virtual string[] ParameterFormer(Node parentNode) // формирование параметров для запроса
        {
            return new string[0];
        }

        protected void LoadDbItem(Node parentNode, string query, SqlConnection connection) // выполнение запроса, формирование атрибутов
        {
            var queryExecuter = new QueryExecuter();
            DataTable executedQuery = queryExecuter.ExecuteQuery(query, connection);
            if (executedQuery.Rows.Count == 0)
                return;
            AddEachNode(parentNode, executedQuery);
        }
    }
}