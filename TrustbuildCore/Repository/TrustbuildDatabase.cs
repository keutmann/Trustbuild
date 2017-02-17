using TrustbuildCore.Service;
using TrustchainCore.Extensions;
using TrustchainCore.Repository;

namespace TrustbuildCore.Repository
{
    public class TrustbuildDatabase : TrustchainDatabase
    {
        public TrustTable _trust = null;
        public TrustTable Trust
        {
            get
            {
                return _trust ?? (_trust = new TrustTable(Connection));
            }
        }

        public TrustbuildDatabase()
        {
        }

        public TrustbuildDatabase(string name)
        {
            Name = name;
        }

        public override void CreateIfNotExist()
        {
            base.CreateIfNotExist();

            Trust.CreateIfNotExist();
        }

        public static TrustbuildDatabase Open()
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
                            MemoryDatabase = new TrustbuildDatabase();
                            MemoryDatabase.OpenConnection();
                            MemoryDatabase.CreateIfNotExist();
                        }
                    }
                }
                return (TrustbuildDatabase)MemoryDatabase;
            }
            else
            {
                var db = new TrustbuildDatabase();
                db.OpenConnection();
                return db;
            }
        }
    }
}
