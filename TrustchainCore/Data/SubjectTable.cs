﻿using Newtonsoft.Json.Linq;
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

            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS " + TableName + "IssuerId ON " + TableName + " (issuerid)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS " + TableName + "Id ON " + TableName + " (id)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS " + TableName + "IdType ON " + TableName + " (idtype)", Connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS " + TableName + "Scope ON " + TableName + " (scope)", Connection);
            command.ExecuteNonQuery();
        }

        public int Add(SubjectModel subject)
        {
            var command = new SQLiteCommand("REPLACE INTO " + TableName + " (issuerid, id, signature, idtype, claim, cost, activate, expire, scope) "+
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

        public IEnumerable<SubjectModel> Select(byte[] issuerId)
        {
            var command = new SQLiteCommand("SELECT * FROM " + TableName + " where issuerid = @issuerid", Connection);
            command.Parameters.Add(new SQLiteParameter("@issuerid", issuerId));

            return Query<SubjectModel>(command, NewItem);
        }

        public SubjectModel NewItem(SQLiteDataReader reader)
        {
            return new SubjectModel
            {
                IssuerId = reader.GetBytes("issuerid"),
                Id = reader.GetBytes("id"),
                Signature = reader.GetBytes("signature"),
                IdType = reader.GetString("idtype"),
                Claims = reader.GetString("claim").DeserializeObject<ClaimModel[]>(),
                Cost = reader.GetInt32(5),
                Activate = reader.GetDateTime(6),
                Expire = reader.GetDateTime(7),
                Scope = reader.GetString("scope")
            };
        }



    }
   
}
