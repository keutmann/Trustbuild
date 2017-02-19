using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TrustchainCore.Data;
using TrustchainCore.Service;

namespace TrustbuildTest
{
    [TestFixture]
    public class TestDBjson : BuildTest
    {

        [Test]
        public void TestJSON()
        {
            using (var db = TrustchainDatabase.Open())
            {
                //var itemsCount = 20000;
                //var name = "Carsten";
                ////var data = new JArray();
                //using (var timer = new TimeMe("Add data"))
                //{


                //    for (int i = 0; i < itemsCount; i++)
                //    {
                //        var key = name + i;
                //        var data = new JObject(
                //                 new JProperty("id", i),
                //                 new JProperty("name", key),
                //                 new JProperty("phone", "1234455454"),
                //                 new JProperty("subname",new JArray(
                //                     new JObject(new JProperty("nick", "Test" + i)),
                //                     new JObject(new JProperty("nick", "Hans" + i)),
                //                     new JObject(new JProperty("nick", "Oles" + i))
                //                     )
                //                 )
                //             );

                //        db.Trust.Add(key, data);
                //    }
                //}

                //using (var timer = new TimeMe("Sub JSON search"))
                //{
                //    var count = 0;

                //    for (int i = 0; i < itemsCount; i++)
                //    {
                //        var result2 = db.Trust.SelectSubJSON("Test" + i);
                //        count++;
                //    }
                //    Console.WriteLine("Number of selects: " + count);
                //}


                //using (var timer = new TimeMe("JSON search"))
                //{
                //    var count = 0;

                //    for (int i = 0; i < itemsCount; i ++)
                //    {
                //        var result2 = db.Trust.SelectJSON("Carsten" + i);
                //        count++;
                //    }
                //    Console.WriteLine("Number of selects: " + count);
                //}

                //using (var timer = new TimeMe("SQL search"))
                //{
                //    var count = 0;

                //    for (int i = 0; i < itemsCount; i ++)
                //    {
                //        var result2 = db.Trust.SelectSQL("Carsten" + i);
                //        count++;
                //    }
                //    Console.WriteLine("Number of selects: " + count);
                //}


                //Assert.NotNull(result2);
                //Assert.IsTrue(result.Count == data.Count);
            }

        }
    }
}
