using System;
using System.Data.SqlClient;
using DatabaseLibrary.DatabaseNodes;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary
{
    public class DatabaseReader
    {
        public ConnectionData ConnectionData { private get; set; }

        public Node DatabaseCreator(string dbName = null) //создание корневого узла баз + запуск создания базы либо всех баз на сервере
        {
            Node root = new Node(GroupItem.Databases);
            using (var connection = ConnectionData.CreateConnection())
            {
                if (dbName != null)
                {
                    DbReader reader = new DbReader();
                    reader.CreateDb(root, connection, dbName);
                }
                CreateFromNode(root, true);
            }
            return root;
        }

        private void CreateFromNode(Node node, bool startFromChild = false)
        {
            foreach (var child in node)
            {
                if (startFromChild)//если создаем одну только базу
                {
                    startFromChild = false;
                    continue;
                }
                LazyLoader lazyLoader = new LazyLoader();
                lazyLoader.LoadNode(child, ConnectionData);
            }
        }

    }
}