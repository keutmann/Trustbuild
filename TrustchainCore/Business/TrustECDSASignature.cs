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
        protected TrustModel Trust { get; set; }

        public TrustECDSASignature(TrustModel trust)
        {
            this.Trust = trust;
        }

        public List<string> VerifyTrustSignature()
        {
            var Errors = new List<string>();

            if (Trust.Issuer.Signature == null || Trust.Issuer.Signature.Length == 0)
            {
                Errors.Add("Missing issuer signature");
                return Errors;
            }

            var trustHash = new uint256(Trust.TrustId);

            if (VerifySignature(trustHash, Trust.Issuer.Signature, Trust.Issuer.Id))
            {
                Errors.Add("Invalid issuer signature");
                return Errors;
            }
            

            foreach (var subject in Trust.Issuer.Subjects)
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



        public static byte[] GetHashOfBinary(byte[] data)
        {
            return Hashes.SHA256(Hashes.SHA256(data));
        }

        public bool VerifySignature(uint256 hashkeyid, byte[] signature, byte[] address)
        {
            var recoverAddress = PubKey.RecoverCompact(hashkeyid, signature);

            return recoverAddress.Hash.ToBytes().Compare(address) == 0;

        }
    }
}
