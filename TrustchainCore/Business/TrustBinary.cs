using NBitcoin.Crypto;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using TrustchainCore.Extensions;
using TrustchainCore.Model;
using TrustchainCore.Security.Cryptography;

namespace TrustchainCore.Business
{
    public class TrustBinary : ITrustBinary
    {
        protected TrustModel Trust { get; set; }

        public TrustBinary(TrustModel trust)
        {
            this.Trust = trust;
        }

        public byte[] GetIssuerBinary()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var issuer = Trust.Issuer;
                ms.WriteBytes(issuer.Id);
                foreach (var subject in issuer.Subjects)
                {
                    ms.WriteBytes(subject.Id);
                    ms.WriteString(subject.IdType);
                    foreach (var claim in subject.Claims)
                    {
                        ms.WriteString(claim.Type);
                        ms.WriteString(claim.Data);
                    }
                    ms.WriteInteger(subject.Cost);
                    //ms.WriteInteger(subject.Activate);
                    //ms.WriteInteger(subject.Expire);
                    ms.WriteString(subject.Scope);
                }

                return ms.ToArray();
            }
        }
    }
}
