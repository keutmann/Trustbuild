using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Business;
using TrustchainCore.Model;
using TrustchainCore.Extensions;
using TrustbuildTest.Resources;
using NBitcoin.Crypto;
using NBitcoin;

namespace TrustbuildTest.Data
{
    [TestFixture]
    public class TrustProofTest
    {



        [Test]
        public void LoadTrust()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);
            Assert.IsTrue(trust != null);
            Assert.IsTrue(trust.Issuer != null);
            Assert.IsTrue(trust.Issuer.Subjects != null);
            Assert.IsTrue(trust.Issuer.Subjects.Length > 0);
        }


        [Test]
        public void VerfifyIssuerSignature()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);
            Assert.IsTrue(trust != null);

            var key32 = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes("Trustchain")));
            var adr32 = key32.PubKey.GetAddress(Network.TestNet);

            Console.WriteLine("PubKey: " + key32.PubKey.ToString());
            Console.WriteLine("Issuer address: " + adr32.ToWif());
            Console.WriteLine("Issuer address base64: " + Convert.ToBase64String(adr32.Hash.ToBytes()));
            trust.Issuer.Id = adr32.Hash.ToBytes();


            var trustBinary = new TrustBinary(trust);
            var hashkeyid = Hashes.Hash256(Hashes.SHA256(trustBinary.GetIssuerBinary()));
            //var hashkeyid2 = new uint256(Hashes.Hash256(trustBinary.GetIssuerBinary()));
            //var signature = key32.SignCompact(hashkeyid);
            trust.Signature.Issuer = key32.SignCompact(hashkeyid);
            Console.WriteLine("Issuer signature: "+Convert.ToBase64String(trust.Signature.Issuer));

            //var recoverAdr = PubKey.RecoverCompact(hashkeyid, trust.Signature.Issuer);
            //bool result = recoverAdr.Hash.ToBytes().Compare(trust.Issuer.Id) == 0; // == adr32.Hash;

            var ecdsaSignature = new TrustECDSASignature(trust, trustBinary);
            var result = ecdsaSignature.VerfifyIssuerSignature();

            Assert.IsTrue(result);
        }

        [Test]
        public void VerfifyTrustSignature()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);
            Assert.IsTrue(trust != null);

            var trustBinary = new TrustBinary(trust);
            var ecdsaSignature = new TrustECDSASignature(trust, trustBinary);
            var result = ecdsaSignature.VerfifyIssuerSignature();

            Assert.IsTrue(result);
        }
    }
}
