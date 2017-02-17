using System;
using System.Data.SQLite;
using System.IO;
using TrustchainCore.Configuration;
using TrustchainCore.Extensions;


namespace TrustchainCore.Repository
{
    public class TrustchainDatabase : IDisposable
    {
        public static string MemoryConnectionString = "Data Source=:memory:;Version=3;";
        public static TrustchainDatabase MemoryDatabase;
        public static object lockObject = new object();

        public static volatile bool IsMemoryDatabase = false;

        public SQLiteConnection Connection;

        public string Name { get; set; }

        public TrustchainDatabase()
        {
        }


        public TrustchainDatabase(string name)
        {
            Name = name;
        }

        public virtual void CreateIfNotExist()
        {
            if (!IsMemoryDatabase && !File.Exists(Connection.FileName))
                SQLiteConnection.CreateFile(Connection.FileName);
        }

        public virtual SQLiteConnection OpenConnection()
        {
            if(IsMemoryDatabase)
            {
                Connection = new SQLiteConnection(MemoryConnectionString);
                Connection.Open();
                Connection.EnableExtensions(true);
                Connection.LoadExtension("SQLite.Interop.dll", "sqlite3_json_init");
                return Connection;
            }

            if(!string.IsNullOrEmpty(App.Config["dbconnectionstring"].ToStringValue()))
            {
                Connection = new SQLiteConnection(App.Config["dbconnectionstring"].ToStringValue());
                Connection.Open();
                return Connection;
            }

            var dbFilename = (!string.IsNullOrEmpty(App.Config["dbfilename"].ToStringValue())) ? App.Config["dbfilename"].ToStringValue() : Name;
            if (!string.IsNullOrEmpty(dbFilename))
            {
                var sb = new SQLiteConnectionStringBuilder();

                sb.DataSource = (string)App.Config["dbfilename"];
                var dbObject = App.Config["database"];
                sb.Flags = SQLiteConnectionFlags.UseConnectionPool;
                //tt.NoSharedFlags = false;

                sb.JournalMode = (SQLiteJournalModeEnum)dbObject["journalmode"].ToInteger((int)SQLiteJournalModeEnum.Default);
                sb.Pooling = dbObject["pooling"].ToBoolean(true);
                sb.ReadOnly = dbObject["readonly"].ToBoolean(false);
                sb.Add("cache", dbObject["cache"].ToStringValue("shared"));
                sb.Add("Compress", dbObject["compress"].ToStringValue("False"));
                sb.SyncMode = (SynchronizationModes)dbObject["syncmode"].ToInteger((int)SynchronizationModes.Normal);

                //sb.DefaultIsolationLevel = System.Data.IsolationLevel.ReadUncommitted;
                //tt.DefaultDbType = System.Data.DbType.
                //var dd = new SQLiteConnection(;

                Connection = new SQLiteConnection(sb.ConnectionString);
                Connection.Open();
                return Connection;
            }



            throw new ApplicationException("Not database connection found");
        }


        public virtual void Dispose()
        {
            if (!IsMemoryDatabase)
                Connection.Dispose();
        }
    }
}
