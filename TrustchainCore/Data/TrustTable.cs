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
                "version TEXT,"+
                "script TEXT,"+
                "issuerid BLOB," +
                "issuersignature BLOB," +
                "serverid BLOB," +
                "serversignature BLOB,"+
                "timestamp TEXT"+
                ")";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();

            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS " + TableName+ "IssuerId ON " + TableName + " (issuerid)", Connection);
            command.ExecuteNonQuery();
        }

        public int Add(Trust trust)
        {
            var command = new SQLiteCommand("INSERT INTO " + TableName + " (version, script, issuerid, issuersignature, serverid, serversignature, timestamp) "+ 
                "VALUES (@version, @script, @issuerid, @issuersignature, @serverid, @serversignature, @timestamp)", Connection);
            command.Parameters.Add(new SQLiteParameter("@version", trust.Head.Version));
            command.Parameters.Add(new SQLiteParameter("@script", trust.Head.Script));
            command.Parameters.Add(new SQLiteParameter("@issuerid", trust.Issuer.Id));
            command.Parameters.Add(new SQLiteParameter("@issuersignature", trust.Issuer.Signature));
            command.Parameters.Add(new SQLiteParameter("@serverid", trust.Server.Id));
            command.Parameters.Add(new SQLiteParameter("@serversignature", trust.Server.Signature));
            command.Parameters.Add(new SQLiteParameter("@timestamp", trust.Timestamp.SerializeObject()));
            return command.ExecuteNonQuery();
        }

        public Trust Select(byte[] issuerId)
        {
            var command = new SQLiteCommand("SELECT * FROM " + TableName + " where issuerid = @issuerid", Connection);
            command.Parameters.Add(new SQLiteParameter("@issuerid", issuerId));

            return Query<Trust>(command, NewItem).FirstOrDefault();
        }

        public Trust NewItem(SQLiteDataReader reader)
        {
            return new Trust
            {
                Head = new Head
                {
                    Version = reader.GetString("version"),
                    Script = reader.GetString("script")
                },
                Issuer = new Issuer
                {
                    Id = reader.GetBytes("issuerid"),
                    Signature = reader.GetBytes("issuersignature")
                },
                Server = new Server
                {
                    Id = reader.GetBytes("serverid"),
                    Signature = reader.GetBytes("serversignature")
                },
                Timestamp = reader.GetString("timestamp").DeserializeObject<Timestamp[]>()
            };
        }
    }
}
