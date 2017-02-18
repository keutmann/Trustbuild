using NUnit.Framework;
using TrustchainCore.Data;

namespace TrustbuildTest
{
    public abstract class BuildTest
    {
        [SetUp]
        public virtual void Init()
        {
            using (var db = TrustchainDatabase.Open())
            {
                db.CreateIfNotExist();
            }
        }

        [TearDown]
        public virtual void Dispose()
        { /* ... */ }
    }
}
