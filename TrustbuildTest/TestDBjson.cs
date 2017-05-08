using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Service;

namespace TrustbuildTest
{
    [TestFixture]
    public class TestDBjson : BuildTest
    {

        [Test]
        public void TestJSON()
        {

            var json = @"{ ""head"":{ ""version"":""standard 0.1.0"",""script"":""btc-pkh""},""issuer"":[{""id"":""RzUTJlQoHLzgtXjKiBx4+QAg1aQ="",""signature"":""MEQCICKPVHZv+MfbARVG4jQMswliTms4FHlQz2lYZssGiF+NAiB0/mLZwl6xJfpFCCdsOWiScqZLODgXisST3pwfQN6qIg=="",""subject"":[{""id"":""RzUTJlQoHLzgtXjKiBx4+QAg1aQ="",""idtype"":""identity"",""scope"":""reddit"",""claim"":{""trust"":true},""cost"":100,""activate"":0,""expire"":0},{""id"":""K2c618oiqO547JJ9bWs6lsKFWCI="",""idtype"":""name"",""scope"":""reddit"",""claim"":{""trust"":true},""cost"":100,""activate"":0,""expire"":0}]}]}";

            var obj = JsonConvert.DeserializeObject<TrustModel>(json);

            Assert.NotNull(obj);
            //using (var db = TrustchainDatabase.Open())
            //{
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
            //}

        }
    }
}
