using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary
{
    public class ConnectionData
    {
        private const string DefaultServer = "localhost\\SQLEXPRESS"/*"user-pc"*/;

        private  SqlConnectionStringBuilder Builder { get; set; }

        public string Login { get; private set; }

        public string Password { get; private set; }

        public string ServerName { get; private set; }



        public void CreateConnectionString(string server, string login, string pass)//подключение к серверу через sql
        {
            CreateConnectionString(server);
            Builder.IntegratedSecurity = false;
            if (login != null)
            {
                Builder.UserID = login;
                Login = login;
            }
            if (pass != null)
            {
                Builder.Password = pass;
                Password = pass;
            }

        }

        public void CreateConnectionString(string server)//подключение к серверу через windows auth
        {
            ServerName = server.Equals("default") ? DefaultServer : server;
            Builder = new SqlConnectionStringBuilder
            {
                DataSource = ServerName,
                ConnectTimeout = 5,
                IntegratedSecurity = true
            };
        }


        public SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(Builder.ConnectionString);
            connection.Open();
            return connection;
        }

        public SqlConnection CreateConnectionAsync(CancellationTokenSource tokenSource)
        {
            var connection = new SqlConnection(Builder.ConnectionString);
            var result = connection.OpenAsync(tokenSource.Token);
            result.Wait();
            return connection;
        }

        public Task<bool> CheckServerConnectionAsync(CancellationTokenSource tokenSource)
        {
            return Task.Run(() =>
            {
                try
                {
                    var result = CreateConnectionAsync(tokenSource);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }, tokenSource.Token);
        }

        public bool CheckServerConnection()// проверка соединения с сервером
        {
            using (CreateConnection())
            {
                return true;
            }
        }

        public void ConnectToDb(string dbName)// подключение к конкретной БД
        {
            if (CheckDbExist(dbName))
                Builder.InitialCatalog = dbName;
        }

        private bool CheckDbExist(string dbName)// проверка существования БД
        {
            using (var connection = CreateConnection())
            {
                string query = $"SELECT DB_ID('{dbName}')";
                var command = new SqlCommand(query, connection);
                var result = command.ExecuteScalar();
                if (result is DBNull)
                    throw new Exception("Cannot connect to the database, it doesn't exist");
            }
            return true;
        }

        public string GetNewInitialCatalog(Node node)
        {
            var dbName = node.FindAncestor(SingleItem.Database)?.GetAttributeValue(ObjectAttribute.Name);
            return dbName;
        }
    }
}
