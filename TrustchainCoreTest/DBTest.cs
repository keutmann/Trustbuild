using NUnit.Framework;
using TrustchainCore.Data;

namespace TrustchainCoreTest
{
    public abstract class DBTest
    {
        [SetUp]
        public virtual void Init()
        {
            using (var db = TrustchainDatabase.Open())
            {
                // Make sure that the in memory database exist
                db.CreateIfNotExist();
            }
        }

        [TearDown]
        public virtual void Dispose()
        { /* ... */ }
    }
}
