using NUnit.Framework;
using System;
using TrustchainCore.Collections;

namespace TrustbuildTest
{
    [TestFixture]
    public class TestBloomfilter
    {
        [Test]
        public void TestMethod()
        {

            int capacity = 20000; // the number of items you expect to add to the filter
            int items = 1000; // the number of items you expect to add to the filter
            var filter1 = new BloomFilter<string>(capacity);
            
            Console.WriteLine("Bitarray: " + (filter1.hashBits.Length / 8) / 1024 + " kb");
            // add your items, using:

            for (var i = 0; i < items; i ++)
            {
                filter1.Add("SomeString"+i);
            }
            Console.WriteLine("Trussiness: "+filter1.Truthiness);
            var countfilter1 = 0;
            for (var index = 0; index < filter1.hashBits.Length; index++)
            {
                if (filter1.hashBits[index] == true)
                    countfilter1++;
            }
            Console.WriteLine("Filter1 bits: " + countfilter1);

            
            var filter2 = new BloomFilter<string>(capacity);
            Console.WriteLine("Bitarray: " + (filter2.hashBits.Length / 8) / 1024 + " kb");
            // add your items, using:
            var existin = 0;
            for (var i = items - 50; i < items + 100; i++)
            {
                filter2.Add("SomeString" + i);
                if (filter1.Contains("SomeString" + i))
                    existin++;
            }
            Console.WriteLine("Existin : " + existin);

            var result = filter1.hashBits.And(filter2.hashBits);
            var filter3 = new BloomFilter<string>(capacity);
            filter3.hashBits = result;


            var count = 0;
            var found = 0;
            for(var index = 0; index < result.Length; index++)
            {
                if (result[index] == true)
                    count++;
            }

            for (var i = items - 50; i < items + 100; i++)
            {
                if (filter3.Contains("SomeString" + i))
                    found++;
            }

            Console.WriteLine("Int : " + typeof(int).Name);
            Console.WriteLine("Count : " + count);
            Console.WriteLine("Found : " + found);

            //// now you can check for them, using:
            //if (filter.Contains("SomeString"))
            //    Console.WriteLine("Match!");


            // TODO: Add your test code here
            //Assert.("Your first passing test");
        }
    }
}
