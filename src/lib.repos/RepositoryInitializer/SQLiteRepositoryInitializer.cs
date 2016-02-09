using Ninject;
using System;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace lib.repos.SQLite
{
    using FluentData;
    using lib.logging;
    
    internal sealed class SQLiteRepositoryInitializer : RepositoryInitializerBase
    {
        const string DataDirectoryArg = "|DataDirectory|";

        [Inject]
        public SQLiteRepositoryInitializer(ILogProvider log) : base(log)
        {
            this.Provider = new SqliteProvider();
        }

        public override void Refresh()
        {
            string connectionString = ConfigurationManager
              .ConnectionStrings[this.ConnectionStringName]
              .ConnectionString;

            string databaseFileName = buildDatabaseFileName(connectionString);

            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
                Init();
            }
        }

        public override void Init()
        {
            if (this.IsInitialized) return;

            string connectionString = ConfigurationManager
                .ConnectionStrings[this.ConnectionStringName]
                .ConnectionString;

            string databaseFileName = buildDatabaseFileName(connectionString);

            createDbIfMissing(databaseFileName);
            
            Log.WithLogLevel(LogLevel.Information)
                .WriteMessage("SQLite Database file being used '{0}'", databaseFileName);

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                const string createTableMessageMask = "Created Database Table '{0}'";
                bool createdTable = false;

                conn.Open();

                createdTable = createTableIfMissing(conn, SQL_User_Table_Name, SQL_DDL_Create_User);
                Log.If(createdTable)
                    .WithLogLevel(LogLevel.Information)
                    .WriteMessage(createTableMessageMask, SQL_User_Table_Name);

                createdTable = createTableIfMissing(conn, SQL_UserClaim_Table_Name, SQL_DDL_Create_UserClaim);
                Log.If(createdTable)
                    .WithLogLevel(LogLevel.Information)
                    .WriteMessage(createTableMessageMask, SQL_UserClaim_Table_Name);
            }

            this.IsInitialized = true;
        }

        static void createDbIfMissing(string databaseFileName)
        {
            bool databsaeFileExists = File.Exists(databaseFileName);
            
            if (!databsaeFileExists)
            {
                string dir = Path.GetDirectoryName(databaseFileName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                SQLiteConnection.CreateFile(databaseFileName);
            }
        }

        static string buildDatabaseFileName(string connectionString)
        {
            SQLiteConnectionStringBuilder connBuilder;

            string expandedConnectionString;

            object dataDirectory =
                 AppDomain.CurrentDomain.GetData("DataDirectory");

            if (dataDirectory != null)
            {
                expandedConnectionString = connectionString.Replace(
                   DataDirectoryArg, dataDirectory.ToString());
            }
            else
            {
                expandedConnectionString = connectionString.Replace(
                 DataDirectoryArg, AppDomain.CurrentDomain.BaseDirectory);
            }

            connBuilder = new SQLiteConnectionStringBuilder(expandedConnectionString);

            return connBuilder.DataSource;
        }
        static bool createTableIfMissing(SQLiteConnection conn, string name, string ddl)
        {
            bool result = false;

            if (!doesTableExists(conn, name))
            {
                executeNonQueryCommand(conn, ddl);
                result = true;
            }

            return result;
        }
        static bool doesTableExists(SQLiteConnection conn, string name)
        {
            bool result = false;

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.Connection = conn;

                cmd.CommandText = SQL_Table_Exists_Check;

                SQLiteParameter nameParam = cmd.CreateParameter();
                nameParam.ParameterName = "name";
                nameParam.Value = name;
                nameParam.DbType = DbType.String;

                cmd.Parameters.Add(nameParam);

                result = cmd.ExecuteScalar() != null;
            }

            return result;
        }
        static int executeNonQueryCommand(SQLiteConnection conn, string sql)
        {
            int rowCount = 0;

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.Connection = conn;

                cmd.CommandText = sql;

                rowCount = cmd.ExecuteNonQuery();
            }

            return rowCount;
        }

        const string SQL_Table_Exists_Check = "SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = @name;";
        const string SQL_User_Table_Name = "user";
        const string SQL_DDL_Create_User = "CREATE TABLE user (id INTEGER NOT NULL, username TEXT NOT NULL,password TEXT NOT NULL, PRIMARY KEY(id));";
        const string SQL_UserClaim_Table_Name = "userclaim";
        const string SQL_DDL_Create_UserClaim = "CREATE TABLE userclaim (id INTEGER NOT NULL, username TEXT NOT NULL, claim TEXT NOT NULL, PRIMARY KEY(id));";
    }
}
