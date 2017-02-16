﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustbuildCore.Repository
{
    public class SubjectTable : DBTable
    {
        public SubjectTable(SQLiteConnection connection, string tableName = "Subject")
        {
            Connection = connection;
            TableName = tableName;
        }

        public void CreateIfNotExist()
        {
            if (TableExist())
                return;

            var  sql = "CREATE TABLE IF NOT EXISTS " + TableName + " " +
                "(" +
                "issuerrowid INTEGER," +
                "id BLOB," +
                "signature BLOB,"+
                "index INTEGER," +
                "idtype TEXT,"+
                "claimtype TEXT," +
                "claim TEXT," +
                "cost INTEGER," +
                "activate INTEGER," +
                "expire INTEGER," +
                "scopetype TEXT," +
                "scope TEXT" +
                ")";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();

            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustIssuerRowId ON " + TableName + " (issuerrowid)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectId ON " + TableName + " (id)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectIdType ON " + TableName + " (idtype)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectScope ON " + TableName + " (scope)", Connection);
            command.ExecuteNonQuery();
        }

        /*
                public int Add(JObject proof)
                {
                    var command = new SQLiteCommand("INSERT INTO Proof (hash, path, partition, timestamp) VALUES (@hash,@path,@partition,@timestamp)", Connection);
                    command.Parameters.Add(new SQLiteParameter("@hash", GetByteArray(proof["hash"])));
                    command.Parameters.Add(new SQLiteParameter("@path", GetByteArray(proof["path"])));
                    command.Parameters.Add(new SQLiteParameter("@partition", proof["partition"]));
                    command.Parameters.Add(new SQLiteParameter("@timestamp", (DateTime)proof["timestamp"]));
                    return command.ExecuteNonQuery();
                }

                public JArray Select(int count)
                {
                    var command = new SQLiteCommand("SELECT * FROM Proof ORDER BY partition DESC LIMIT @count", Connection);
                    command.Parameters.Add(new SQLiteParameter("@count", count));
                    return Query(command, NewItem);
                }

                public int UpdatePath(JObject proof)
                {
                    return UpdatePath(GetByteArray(proof["hash"]), GetByteArray(proof["path"]));
                }

                public int UpdatePath(byte[] hash, byte[] path)
                {
                    var command = new SQLiteCommand("UPDATE Proof SET path = @path WHERE hash = @hash", Connection);
                    command.Parameters.Add(new SQLiteParameter("@hash", hash));
                    command.Parameters.Add(new SQLiteParameter("@path", path));
                    return command.ExecuteNonQuery();
                }

                public JObject GetByHash(byte[] hash)
                {
                    var command = new SQLiteCommand("select * from Proof where hash = @hash LIMIT 1", Connection);
                    command.Parameters.Add(new SQLiteParameter("@hash", hash));
                    return (JObject)Query(command, NewItem).FirstOrDefault();
                }

                public int Count()
                {
                    var command = new SQLiteCommand("SELECT count(*) FROM Proof", Connection);
                    var result = Query(command, (reader) => new JObject(new JProperty("count", reader[0]))).FirstOrDefault();
                    return (int)result["count"];
                }

                /// <summary>
                /// Get all batch codes where the proofs has not been build yet. 
                /// Excluding the currrent batch.
                /// </summary>
                /// <param name="excludePartition"></param>
                /// <returns></returns>
                public JArray GetUnprocessedPartitions(string excludePartition)
                {
                    var command = new SQLiteCommand("SELECT DISTINCT partition FROM Proof WHERE (path IS NULL or path = @path) and partition != @partition ORDER BY partition", Connection);
                    command.Parameters.Add(new SQLiteParameter("@partition", excludePartition));
                    command.Parameters.Add(new SQLiteParameter("@path", new byte[0]));

                    return Query(command, (reader) => new JObject(new JProperty("partition", reader["partition"])));
                }

                public JArray GetByPartition(string partition)
                {
                    var command = new SQLiteCommand("SELECT * FROM Proof WHERE partition = @partition", Connection);
                    command.Parameters.Add(new SQLiteParameter("@partition", partition));
                    return Query(command, NewItem);
                }

                public void DropTable()
                {
                    var command = new SQLiteCommand("DROP TABLE Proof", Connection);
                    command.ExecuteNonQuery();
                }

                public JObject NewItem(SQLiteDataReader reader)
                {
                    return NewItem(reader["hash"], reader["path"], reader["partition"], reader["timestamp"]);
                }


                public JObject NewItem(object hash, object path = null, object partition = null, object timestamp = null)
                {
                    return new JObject(
                        new JProperty("hash", hash),
                        new JProperty("path", path),
                        new JProperty("partition", partition),
                        new JProperty("timestamp", timestamp)
                        );
                }
                */

    }
}
