using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Repository;

namespace TrustbuildCore.Repository
{
    public class TrustTable : DBTable
    {
        public TrustTable(SQLiteConnection connection, string tableName = "Trust")
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
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "serverid BLOB," +
                "serversignature BLOB,"+
                "timestamp TEXT,"+
                "name TEXT,"+
                "trustdata JSON"+
                ")";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();

            //command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustName ON Trust (name)", Connection);
            //command.ExecuteNonQuery();

            //command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustJSON ON Trust (json_extract(trustdata, '$.name'))", Connection);
            //command.ExecuteNonQuery();

            //command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustNickJSON ON Trust (json_tree(trustdata, '$.subname')) WHERE json_tree.key='nick'", Connection);
            //command.ExecuteNonQuery();
            //CREATE INDEX json_tbl_idx on json_tbl(json_extract(json, '$.value'))
        }

        public int Add(string name, JToken data)
        {
            var command = new SQLiteCommand("INSERT INTO Trust (name, trustdata) VALUES (@name, @trustdata)", Connection);
            command.Parameters.Add(new SQLiteParameter("@name", name));
            command.Parameters.Add(new SQLiteParameter("@trustdata", data.ToString()));
            return command.ExecuteNonQuery();
        }


        //public int Add(JObject proof)
        //{
        //    var command = new SQLiteCommand("INSERT INTO Trust (serverid, serversignature, timestamp, trust) VALUES (@serverid,@serversignature,@timestamp,@trust)", Connection);
        //    command.Parameters.Add(new SQLiteParameter("@hash", GetByteArray(proof["hash"])));
        //    command.Parameters.Add(new SQLiteParameter("@path", GetByteArray(proof["path"])));
        //    command.Parameters.Add(new SQLiteParameter("@partition", proof["partition"]));
        //    command.Parameters.Add(new SQLiteParameter("@timestamp", (DateTime)proof["timestamp"]));
        //    return command.ExecuteNonQuery();
        //}

        public JArray SelectSQL()
        {
            var command = new SQLiteCommand("SELECT * FROM Trust", Connection);
            //command.Parameters.Add(new SQLiteParameter("@count", count));
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                var data = reader["trustdata"].ToString();
                return JArray.Parse(data);
               }
            return null;
        }

        public object SelectJSON(string name)
        {
            var command = new SQLiteCommand("SELECT json_extract(Trust.trustdata, '$') FROM Trust where json_extract(Trust.trustdata, '$.name') = '" + name + "'", Connection);
            //var command = new SQLiteCommand("SELECT json_extract(Trust.trustdata, '$') FROM Trust", Connection);
            //command.Parameters.Add(new SQLiteParameter("@count", count));
            var test = command.ExecuteScalar();
            return test;

        }


        public object SelectSubJSON(string name)
        {
            var command = new SQLiteCommand("SELECT json_extract(Trust.trustdata, '$') FROM Trust, json_tree(Trust.trustdata, '$.subname') WHERE json_tree.key='nick' and json_tree.value = '" + name + "'", Connection);
            //var command = new SQLiteCommand("SELECT json_extract(Trust.trustdata, '$') FROM Trust", Connection);
            //command.Parameters.Add(new SQLiteParameter("@count", count));
            var test = command.ExecuteScalar();
            return test;

        }
        public object SelectSQL(string name)
        {
            var command = new SQLiteCommand("SELECT trustdata FROM Trust where name = '" + name + "'", Connection);
            //var command = new SQLiteCommand("SELECT json_extract(Trust.trustdata, '$') FROM Trust", Connection);
            //command.Parameters.Add(new SQLiteParameter("@count", count));
            var test = command.ExecuteScalar();
            return test;
            //SQLiteDataReader reader = command.ExecuteReader();
            //if (reader.Read())
            //{
            //    var data = reader[0].ToString();
            //    return JArray.Parse(data);
            //}
            //return null;
        }

        //public int UpdatePath(JObject proof)
        //{
        //    return UpdatePath(GetByteArray(proof["hash"]), GetByteArray(proof["path"]));
        //}

        //public int UpdatePath(byte[] hash, byte[] path)
        //{
        //    var command = new SQLiteCommand("UPDATE Proof SET path = @path WHERE hash = @hash", Connection);
        //    command.Parameters.Add(new SQLiteParameter("@hash", hash));
        //    command.Parameters.Add(new SQLiteParameter("@path", path));
        //    return command.ExecuteNonQuery();
        //}

        //public JObject GetByHash(byte[] hash)
        //{
        //    var command = new SQLiteCommand("select * from Proof where hash = @hash LIMIT 1", Connection);
        //    command.Parameters.Add(new SQLiteParameter("@hash", hash));
        //    return (JObject)Query(command, NewItem).FirstOrDefault();
        //}

        //public int Count()
        //{
        //    var command = new SQLiteCommand("SELECT count(*) FROM Proof", Connection);
        //    var result = Query(command, (reader) => new JObject(new JProperty("count", reader[0]))).FirstOrDefault();
        //    return (int)result["count"];
        //}

        ///// <summary>
        ///// Get all batch codes where the proofs has not been build yet. 
        ///// Excluding the currrent batch.
        ///// </summary>
        ///// <param name="excludePartition"></param>
        ///// <returns></returns>
        //public JArray GetUnprocessedPartitions(string excludePartition)
        //{
        //    var command = new SQLiteCommand("SELECT DISTINCT partition FROM Proof WHERE (path IS NULL or path = @path) and partition != @partition ORDER BY partition", Connection);
        //    command.Parameters.Add(new SQLiteParameter("@partition", excludePartition));
        //    command.Parameters.Add(new SQLiteParameter("@path", new byte[0]));

        //    return Query(command, (reader) => new JObject(new JProperty("partition", reader["partition"])));
        //}

        //public JArray GetByPartition(string partition)
        //{
        //    var command = new SQLiteCommand("SELECT * FROM Proof WHERE partition = @partition", Connection);
        //    command.Parameters.Add(new SQLiteParameter("@partition", partition));
        //    return Query(command, NewItem);
        //}

        //public void DropTable()
        //{
        //    var command = new SQLiteCommand("DROP TABLE Proof", Connection);
        //    command.ExecuteNonQuery();
        //}

        //public JObject NewItem(SQLiteDataReader reader)
        //{
        //    return NewItem(reader["hash"], reader["path"], reader["partition"], reader["timestamp"]);
        //}


        //public JObject NewItem(object trustdata)
        //{
        //    return new JObject(
        //        new JProperty("trustdata", trustdata)
        //        );
        //}


    }
}
