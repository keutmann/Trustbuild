using NUnit.Framework;
using TrustchainCore.Data;

namespace TrustchainCoreTest.Data
{
    [TestFixture]
    public class KeyValueTableTest : DBTest
    {
        [Test]
        public void TestPut()
        {
            using (var db = TrustchainDatabase.Open())
            {
                Assert.IsTrue(db.KeyValue.Put("test", "test") == 1);
                Assert.IsTrue(db.KeyValue.Count() == 1);
            }
        }

        [Test]
        public void TestReplace()
        {
            using (var db = TrustchainDatabase.Open())
            {
                Assert.IsTrue(db.KeyValue.Put("test", "test") == 1);
                Assert.IsTrue(db.KeyValue.Put("test", "test") == 1);
                Assert.IsTrue(db.KeyValue.Count() == 1);
            }
        }

        [Test]
        public void TestGet()
        {
            using (var db = TrustchainDatabase.Open())
            {
                Assert.IsTrue(db.KeyValue.Put("test", "test") == 1);
                Assert.IsTrue(db.KeyValue.Get("test") == "test");
            }
        }

        [Test]
        public void TestDelete()
        {
            using (var db = TrustchainDatabase.Open())
            {
                Assert.IsTrue(db.KeyValue.Put("test", "test") == 1);
                Assert.IsTrue(db.KeyValue.Get("test") == "test");
                Assert.IsTrue(db.KeyValue.Delete("test") == 1);
                Assert.IsTrue(db.KeyValue.Get("test") == null);
                Assert.IsTrue(db.KeyValue.Count() == 0);
            }
        }

    }
}
