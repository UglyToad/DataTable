namespace UglyToad.DataTable.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;

    public class DatabaseBootstrapper
    {
        private static string rootDirectory;
        private static string serverName;
        private static string databaseName;

        static DatabaseBootstrapper()
        {
            rootDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            serverName = ConfigurationManager.AppSettings["serverName"];
            databaseName = ConfigurationManager.AppSettings["databaseName"];
        }

        public static void CreateAndPopulate()
        {
            var databaseBootstrapper = new DatabaseBootstrapper();

            var createFiles = Directory.EnumerateFiles(rootDirectory + ScriptsDirectories.CreateScripts);

            databaseBootstrapper.CreateDatabase(createFiles);

            var updateFiles = Directory.EnumerateFiles(rootDirectory + ScriptsDirectories.UpdateScripts);

            databaseBootstrapper.PopulateDatabase(updateFiles);

            var dataFiles = Directory.EnumerateFiles(rootDirectory + ScriptsDirectories.DataScripts);

            databaseBootstrapper.PutDataInDatabase(dataFiles);

            databaseBootstrapper.ClearPools();
        }

        public static void Drop()
        {
            var databaseBootstrapper = new DatabaseBootstrapper();

            databaseBootstrapper.DropIfExists();

            databaseBootstrapper.ClearPools();
        }

        public static string GetConnectionString()
        {
            var databaseBootstrapper = new DatabaseBootstrapper();

            return databaseBootstrapper.GetConnectionString(true);
        }

        protected virtual void CreateDatabase(IEnumerable<string> createFiles)
        {
            var createScript = GetCreateScript(createFiles);

            DropIfExists();

            RunScript(createScript, GetConnectionString(false));
        }

        protected virtual void PopulateDatabase(IEnumerable<string> updateFiles)
        {
            RunAllScriptsInFolder(updateFiles);
        }

        protected virtual void PutDataInDatabase(IEnumerable<string> dataFiles)
        {
            RunAllScriptsInFolder(dataFiles);
        }

        private void RunAllScriptsInFolder(IEnumerable<string> files)
        {
            var connectionString = GetConnectionString(true);

            foreach (var file in files.OrderBy(uf => uf))
            {
                var sql = File.ReadAllText(file);

                foreach (var script in GetSubScripts(sql))
                {
                    RunScript(script, connectionString);
                }
            }
        }

        private void DropIfExists()
        {
            if (DatabaseExists())
            {
                DropDatabase();
            }
        }

        private void ClearPools()
        {
            SqlConnection.ClearAllPools();
        }

        private string GetCreateScript(IEnumerable<string> createFiles)
        {
            if (createFiles.Count() != 1)
            {
                throw new Exception("Expected only one Create script");
            }

            string script = string.Format(File.ReadAllText(createFiles.First()), databaseName);

            script = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)[0];

            return script;
        }

        public bool DatabaseExists()
        {
            string connectionString = GetConnectionString(false);

            DataTable schemaTable = GetSchemaForServer(connectionString);

            return SchemaContainsDatabase(schemaTable);
        }

        private DataTable GetSchemaForServer(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var schema = connection.GetSchema("Databases");

                connection.Close();

                return schema;
            }
        }

        private bool SchemaContainsDatabase(DataTable schemaTable)
        {
            if (!schemaTable.Columns.Contains("database_name"))
            {
                return false;
            }

            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                if (string.Compare(schemaTable.Rows[i]["database_name"].ToString(), databaseName, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void DropDatabase()
        {
            ClearPools();

            var connectionString = GetConnectionString(false);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = string.Format("drop database [{0}]", databaseName);

                var command = new SqlCommand(sql, connection);

                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
            }
        }

        protected virtual void RunScript(string script, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(script, connection);

                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
            }
        }

        private string GetConnectionString(bool includeDatabase)
        {
            var connectionString = string.Format("Data Source={0};", serverName);

            connectionString += (includeDatabase) ? string.Format("Initial Catalog={0};", databaseName) : string.Empty;

            connectionString += "Integrated Security=true;";

            return connectionString;
        }

        private IEnumerable<string> GetSubScripts(string script)
        {
            return script.Split(new[] { "GO;", "GO" }, StringSplitOptions.RemoveEmptyEntries).Where(s => string.Compare("go", s) != 0);
        }
    }

    internal static class ScriptsDirectories
    {
        public static string CreateScripts = "\\SQLScripts\\Create";
        public static string UpdateScripts = "\\SQLScripts\\Update";
        public static string DataScripts = "\\SQLScripts\\Data";
    }
}
