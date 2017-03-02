using NUnit.Framework;
using System;
using System.Diagnostics;
using TrustchainCore.Data;
using TrustbuildCore.Service;
using NBitcoin;
using System.Text;
using NBitcoin.Crypto;
using TrustbuildCore.Business;
using TrustchainCore.Business;

namespace TrustbuildTest
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Use in memory database
            App.Config["test"] = true; // Run as test, real timestamp not created!

            var serverKey = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes("server")));
            App.Config["serverwif"] = serverKey.GetBitcoinSecret(App.BitcoinNetwork).ToWif();

            AppDirectory.Setup();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //  all tests in the assembly have been run

        }
    }
}