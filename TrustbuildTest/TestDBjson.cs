using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using TrustbuildTest.Resources;
using TrustchainCore.Business;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Service;
using TrustchainCore.Extensions;

namespace TrustbuildTest
{
    [TestFixture]
    public class TestDBjson : BuildTest
    {

        [Test]
        public void TestJSON()
        {

            
            var trust = JsonConvert.DeserializeObject<TrustModel>(TrustSimple.ClientJSON);

            //TrustManager.EnsureTrustId(trust, new TrustBinary(trust));
            trust.TrustId = Hashes.Hash256(Encoding.UTF8.GetBytes("Demo")).ToBytes();

            Console.WriteLine("Hash Value: "+Encoders.Base64.EncodeData(trust.TrustId));

            var serverKey = new Key(Hashes.Hash256(Encoding.UTF8.GetBytes("server")).ToBytes());
            Console.WriteLine("server key: " + serverKey.GetBitcoinSecret(Network.Main).ToWif());
            var adr = serverKey.ScriptPubKey.GetDestinationAddress(Network.Main);
            Console.WriteLine("server Adr: "+adr.ToWif());
            trust.Issuer.Id = serverKey.ScriptPubKey.GetDestinationAddress(Network.Main).ToBytes();
            Console.WriteLine("TrustID: " + Encoders.Base64.EncodeData(trust.TrustId));
            var sig = TrustECDSASignature.SignMessage(serverKey, trust.TrustId);
            trust.Issuer.Signature = sig;
            Console.WriteLine("Server Signature: " + Encoders.Base64.EncodeData(sig));

            var signature = new TrustECDSASignature(trust);
            
            var manager = new TrustManager();

             manager.VerifyTrust(trust);
             Assert.NotNull(trust);

            Console.WriteLine("Client Address: 1PoRUSi6cZfmX5CbiB4qo4VFA6D12byofu");

            //Console.WriteLine("Client Private key: L2ULKExrzucfUED4vaoezawAEEgAAXtHLLGmFi8k6JKBw4iuFpdG");
            //Console.WriteLine("Client Hash value: S3mtngHqVQD0NzP4URKqLLM9wWeBfRenzlqhEZ5zoC8=");
            //Console.WriteLine("Client Message: yyFz7Hj+P6s1qvzReaxBzkrrNJs70mI5MatXp3VWvos=");
            //Console.WriteLine("Client Pre HEX: 44656d6f");
            //Console.WriteLine("CLient Signature: H+IT0mBp6Ddlak67LeBV83mC3kKTB5uTULa//+mdQIbwZf2tNgSgnVVxEsd9B8KKVf9tO0/ebYyPzFazS8SPRaI=");
        }
    }
}
