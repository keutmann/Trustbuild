using Newtonsoft.Json.Linq;
using System;
using System.IO;
using TrustchainCore.Extensions;
using TrustchainCore.Model;
using TrustchainCore.Security.Cryptography;

namespace TrustchainCore.Data
{
    public class TrustHash
    {
        public static Func<byte[], byte[]> HashStrategy = Crypto.HashStrategy;

        protected Trust trust { get; set; }

        public TrustHash(Trust t)
        {
            trust = t;
        }

        public byte[] GetIssuerBinary()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var issuer = trust.Issuer;
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

        public byte[] GetIssuerHash()
        {
            return HashStrategy(GetIssuerBinary());
        }
    }
}
