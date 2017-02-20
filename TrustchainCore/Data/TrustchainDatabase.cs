using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using TrustchainCore.Configuration;
using TrustchainCore.Extensions;
using TrustchainCore.Model;

namespace TrustchainCore.Data
{
    public class TrustchainDatabase : IDisposable
    {
        public static string MemoryConnectionString = "Data Source=:memory:;Version=3;";
        public static TrustchainDatabase MemoryDatabase;
        public static object lockObject = new object();
        private static volatile bool created = true;

        public static volatile bool IsMemoryDatabase = false;

        public SQLiteConnection Connection;

        public string Name { get; set; }

        public TrustTable _trust = null;
        public TrustTable Trust
        {
            get
            {
                return _trust ?? (_trust = new TrustTable(Connection));
            }
        }

        public SubjectTable _subject = null;
        public SubjectTable Subject
        {
            get
            {
                return _subject ?? (_subject = new SubjectTable(Connection));
            }
        }

        //public TrustchainDatabase(string name)
        //{
        //    Name = name;
        //}

        public virtual string GetDatabaseName()
        {
            return (!string.IsNullOrEmpty(App.Config["dbfilename"].ToStringValue())) ? App.Config["dbfilename"].ToStringValue() : "test.db";
        }

        public virtual void CreateIfNotExist()
        {
            if (!IsMemoryDatabase && !File.Exists(Connection.FileName))
                SQLiteConnection.CreateFile(Connection.FileName);

            Trust.CreateIfNotExist();
            Subject.CreateIfNotExist();
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

        public static TrustchainDatabase Open()
        {
            if (App.Config["test"].ToBoolean())
                IsMemoryDatabase = true;

            if (IsMemoryDatabase)
            {
                if (MemoryDatabase == null)
                {
                    lock (lockObject)
                    {
                        if (MemoryDatabase == null)
                        {
                            MemoryDatabase = new TrustchainDatabase();
                            MemoryDatabase.OpenConnection();
                            MemoryDatabase.CreateIfNotExist();
                        }
                    }
                }
                return MemoryDatabase;
            }
            else
            {
                var db = new TrustchainDatabase();
                db.OpenConnection();
                return db;
            }
        }

        public int AddTrust(Trust trust)
        {
            var result = Trust.Add(trust);
            if (result < 1)
                return result;

            foreach (var subject in trust.Issuer.Subjects)
            {
                subject.IssuerId = trust.Issuer.Id;
                subject.Signature = trust.Signature.Subject[subject.Index].Sig;
                result = Subject.Add(subject);
                if (result < 1)
                    break;
            }
            return result;
        }

        public Trust GetTrust(byte[] issuerid)
        {
            var result = Trust.Select(issuerid);
            var subjects = Subject.Select(issuerid);
            result.Issuer.Subjects = subjects.ToArray();

            var index = 0;
            result.Signature.Subject = (from p in subjects
                                        select new Proof
                                        {
                                            Index = p.Index = index++,
                                            Sig = p.Signature
                                        }).ToArray();
            return result;
        }

        public virtual void Dispose()
        {
            if (!IsMemoryDatabase)
                Connection.Dispose();
        }
    }
}
