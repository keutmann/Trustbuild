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
                "issuerid BLOB," +
                "id BLOB," +
                "signature BLOB,"+
                "idtype TEXT,"+
                "claim TEXT," +
                "cost INTEGER," +
                "activate DATETIME," +
                "expire DATETIME," +
                "scope TEXT" +
                ")";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();

            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectIssuerId ON " + TableName + " (issuerid)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectId ON " + TableName + " (id)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectIdType ON " + TableName + " (idtype)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS TrustSubjectScope ON " + TableName + " (scope)", Connection);
            command.ExecuteNonQuery();
        }

        public int Add(Subject subject)
        {
            var command = new SQLiteCommand("INSERT INTO " + TableName + " (issuerid, id, signature, idtype, claim, cost, activate, expire, scope) "+
                "VALUES (@issuerid, @id, @signature, @idtype, @claim, @cost, @activate, @expire, @scope)", Connection);

            command.Parameters.Add(new SQLiteParameter("@issuerid", subject.IssuerId));
            command.Parameters.Add(new SQLiteParameter("@id", subject.Id));
            command.Parameters.Add(new SQLiteParameter("@signature", subject.Signature));
            command.Parameters.Add(new SQLiteParameter("@idtype", subject.IdType));
            command.Parameters.Add(new SQLiteParameter("@claim", subject.Claims.SerializeObject()));
            command.Parameters.Add(new SQLiteParameter("@cost", subject.Cost));
            command.Parameters.Add(new SQLiteParameter("@activate", subject.Activate));
            command.Parameters.Add(new SQLiteParameter("@expire", subject.Expire));
            command.Parameters.Add(new SQLiteParameter("@scope", subject.Scope));
            return command.ExecuteNonQuery();
        }

        public IEnumerable<Subject> Select(byte[] issuerId)
        {
            var command = new SQLiteCommand("SELECT * FROM " + TableName + " where issuerid = @issuerid", Connection);
            command.Parameters.Add(new SQLiteParameter("@issuerid", issuerId));

            return Query<Subject>(command, NewItem);
        }

        public Subject NewItem(SQLiteDataReader reader)
        {
            return new Subject
            {
                IssuerId = reader.GetBytes("issuerid"),
                Id = reader.GetBytes("id"),
                Signature = reader.GetBytes("signature"),
                IdType = reader.GetString("idtype"),
                Claims = reader.GetString("claim").DeserializeObject<Claim[]>(),
                Cost = reader.GetInt32(5),
                Activate = reader.GetDateTime(6),
                Expire = reader.GetDateTime(7),
                Scope = reader.GetString("scope")
            };
        }



    }
   
}
