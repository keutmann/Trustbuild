using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Extensions;
using TrustchainCore.Model;

namespace TrustchainCore.Business
{
    public class TrustECDSASignature
    {
        protected Trust trust { get; set; }
        protected ITrustBinary Binary { get; set; }

        public TrustECDSASignature(Trust t, ITrustBinary trustBinary)
        {
            trust = t;
            Binary = trustBinary;
        }

        public bool VerfifyIssuerSignature()
        {
            if (trust.Signature.Issuer == null || trust.Signature.Issuer.Length == 0)
                return false;

            var hashkeyid = Hashes.Hash256(Hashes.SHA256(Binary.GetIssuerBinary()));


            var recoverAddress = PubKey.RecoverCompact(hashkeyid, trust.Signature.Issuer);

            return recoverAddress.Hash.ToBytes().Compare(trust.Issuer.Id) == 0;
        }


        public void SignIssuer(byte[] signature)
        {

        }
    }
}
