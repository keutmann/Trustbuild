using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustbuildCore.Repository;

namespace TrustbuildTest
{
    public abstract class BuildTest
    {
        [SetUp]
        public virtual void Init()
        {
            using (var db = TrustbuildDatabase.Open())
            {
                db.CreateIfNotExist();
            }
        }

        [TearDown]
        public virtual void Dispose()
        { /* ... */ }
    }
}
