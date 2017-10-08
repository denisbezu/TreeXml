using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using TreeWPF.Enums;

namespace TreeWPF.Helpers
{
    public class ServersLoader
    {
        public static IEnumerable<string> ListLocalSqlInstances()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }

                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                foreach (string item in ListLocalSqlInstances(Registry.LocalMachine))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<string> ListLocalSqlInstances(RegistryKey hive)
        {
            const string keyName = @"Software\Microsoft\Microsoft SQL Server";
            const string valueName = "InstalledInstances";
            const string defaultName = "MSSQLSERVER";

            using (var key = hive.OpenSubKey(keyName, false))
            {
                if (key == null) return Enumerable.Empty<string>();

                var value = key.GetValue(valueName) as string[];
                if (value == null) return Enumerable.Empty<string>();

                for (int index = 0; index < value.Length; index++)
                {
                    if (string.Equals(value[index], defaultName, StringComparison.OrdinalIgnoreCase))
                    {
                        value[index] = $@"{Environment.MachineName}\";
                    }
                    else
                    {
                        value[index] = $@"{Environment.MachineName}\" + value[index];
                    }
                }

                return value;
            }
        }

        public static void LoadServers(ObservableCollection<string> servers)
        {
            List<string> localServers = ListLocalSqlInstances().ToList();
            foreach (var localServer in localServers)
                servers.Add(localServer);
            LoadNetwork(servers, localServers);
        }

        private static void LoadNetwork(ObservableCollection<string> servers, List<string> localServers)
        {
            Task.Run(() =>
            {
                List<string> availableServers = GetServerInstances(ServerType.Network);
                availableServers = availableServers.OrderBy(server => server).ToList();
                foreach (var availableServer in availableServers)
                {
                    if (!localServers.Contains(availableServer))
                        App.Current.Dispatcher.Invoke(() =>
                            {
                                servers.Add(availableServer);
                            });

                }
            });
        }

        private static List<string> GetServerInstances(ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.Local:
                    return ListLocalSqlInstances().ToList();
                case ServerType.Network:
                    {
                        SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
                        DataTable table = instance.GetDataSources();
                        List<string> serversList = CreateData(table);
                        return serversList;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(serverType), serverType, null);
            }
        }
        
        private static List<string> CreateData(DataTable table)
        {
            List<string> servers = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                servers.Add($"{row["ServerName"]}\\{row["InstanceName"]}");
            }
            return servers;
        }
    }
}