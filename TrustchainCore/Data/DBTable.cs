﻿using Newtonsoft.Json.Linq;
using System;
using System.Data.SQLite;

namespace TrustchainCore.Data
{
    public class DBTable
    {
        public SQLiteConnection Connection { get; set; }
        public string  TableName { get; set; }

        public bool TableExist()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='@table' COLLATE NOCASE";
            var command = new SQLiteCommand(sql, Connection);
            command.Parameters.Add(new SQLiteParameter("@table", TableName));
            var reader = command.ExecuteReader();
            return (reader.Read());
        }

        public JArray Query(SQLiteCommand command, Func<SQLiteDataReader, JObject> newItemMethod = null)
        {
            if (newItemMethod == null)
                throw new MissingMethodException("Missing newItemMethod");

            var result = new JArray();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(newItemMethod(reader));

            return result;
        }

        public byte[] GetByteArray(JToken token)
        {
            return token.Type == JTokenType.Null ? new byte[0] : (byte[])token;
        }
    }
}
