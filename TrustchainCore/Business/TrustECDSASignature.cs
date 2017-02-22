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

        public List<string> VerifyTrustSignature()
        {
            var Errors = new List<string>();

            if (trust.Issuer.Signature == null || trust.Issuer.Signature.Length == 0)
            {
                Errors.Add("Missing issuer signature");
                return Errors;
            }

            var trustHash = GetHashOfBinary(Binary.GetIssuerBinary());

            if (VerifySignature(trustHash, trust.Issuer.Signature, trust.Issuer.Id))
            {
                Errors.Add("Invalid issuer signature");
                return Errors;
            }
            

            foreach (var subject in trust.Issuer.Subjects)
            {
                if (subject.Signature == null || subject.Signature.Length == 0)
                    continue;

                if (!VerifySignature(trustHash, subject.Signature, subject.Id))
                {
                    Errors.Add("Invalid issuer signature");
                    return Errors;
                }
                    
            }
            return Errors;
        }



        public static uint256 GetHashOfBinary(byte[] data)
        {
            return Hashes.Hash256(Hashes.SHA256(data));
        }

        public bool VerifySignature(uint256 hashkeyid, byte[] signature, byte[] address)
        {
            var recoverAddress = PubKey.RecoverCompact(hashkeyid, signature);

            return recoverAddress.Hash.ToBytes().Compare(address) == 0;

        }
    }
}
