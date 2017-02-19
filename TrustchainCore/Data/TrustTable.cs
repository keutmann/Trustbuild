using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Extensions;
using TrustchainCore.Model;

namespace TrustchainCore.Data
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
                "issuerid BLOB," +
                "issuersignature BLOB," +
                "serverid BLOB," +
                "serversignature BLOB,"+
                "timestamp TEXT,"+
                ")";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();

            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustIssuerId ON Trust (issuerid)", Connection);
            command.ExecuteNonQuery();
        }

        public int Add(Trust trust)
        {
            var command = new SQLiteCommand("INSERT INTO Trust (issuerid, issuersignature, serverid, serversignature, timestamp) VALUES (@issuerid, @issuersignature, @serverid, @serversignature, @timestamp)", Connection);
            command.Parameters.Add(new SQLiteParameter("@issuerid", trust.Issuer.Id));
            command.Parameters.Add(new SQLiteParameter("@issuersignature", trust.Signature.Issuer));
            command.Parameters.Add(new SQLiteParameter("@serverid", trust.Server.Id));
            command.Parameters.Add(new SQLiteParameter("@serversignature", trust.Signature.Server));
            command.Parameters.Add(new SQLiteParameter("@timestamp", trust.Timestamp.SerializeObject()));
            return command.ExecuteNonQuery();
        }


        public Trust Select(byte[] issuerId)
        {
            var command = new SQLiteCommand("SELECT * FROM Trust where issuerid = @issuerid", Connection);
            command.Parameters.Add(new SQLiteParameter("@issuerid", issuerId));

            return Query<Trust>(command, NewItem).FirstOrDefault();
        }

        //public JObject GetByHash(byte[] hash)
        //{
        //    var command = new SQLiteCommand("select * from Proof where hash = @hash LIMIT 1", Connection);
        //    command.Parameters.Add(new SQLiteParameter("@hash", hash));
        //    return (JObject)Query(command, NewItem).FirstOrDefault();
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

        public Trust NewItem(SQLiteDataReader reader)
        {
            return new Trust
            {
                Issuer = new Issuer
                {
                    Id = (byte[])reader["issuerid"],
                },
                Server = new Server
                {
                    Id = (byte[])reader["serverid"]
                },
                Timestamp = ((string)reader["timestamp"]).DeserializeObject<Timestamp[]>(),
                Signature = new Signature
                {
                    Issuer = (byte[])reader["issuersignature"],
                    Server = (byte[])reader["serversignature"],
                }

            };
        }


        //public JObject NewItem(object trustdata)
        //{
        //    return new JObject(
        //        new JProperty("trustdata", trustdata)
        //        );
        //}


    }
}
