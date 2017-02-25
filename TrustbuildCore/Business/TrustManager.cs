using Newtonsoft.Json;
using System;
using System.Linq;
using TrustbuildCore.Service;
using TrustchainCore.Business;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Extensions;
using NBitcoin;
using System.IO;

namespace TrustbuildCore.Business
{
    public class TrustManager
    {
        public DateTime Timestamp = DateTime.Now;

        public void AddNew(string content)
        {
            var trust = JsonConvert.DeserializeObject<TrustModel>(content);

            EnsureTrustId(trust, new TrustBinary(trust));

            VerifyTrust(trust);

            // This is the servers task for later processing
            trust.Timestamp = null; 

            AddToDatabase(trust);
        }

        public void AddToDatabase(TrustModel trust)
        {
            var dbname = GetCurrentDBTrustname(trust.Server.Id);
            var fullpath = Path.Combine(App.Config["buildpath"].ToStringValue(), dbname);

            using (var db = TrustchainDatabase.Open(fullpath))
            {
                db.CreateIfNotExist();

                var item = db.Trust.SelectOne(trust.TrustId);
                if (item != null)
                    return; // The trust already exist in the db.

                AddTrust(trust, db);
            }
        }

        public string GetCurrentDBTrustname(byte[] serverid)
        {
            var key = new KeyId(serverid);
            var preName = key.GetAddress(App.BitcoinNetwork).ToWif();
            return string.Format("{0}_{1}.trust", preName, DefaultPartition());
        }

        public string DefaultPartition()
        {
            return GetPartition(Timestamp);
        }

        public string GetPartition(DateTime datetime)
        {
            return datetime.ToString(App.Config["partition"].ToStringValue("yyyyMMddhh0000"));
        }

        public int AddTrust(TrustModel trust, TrustchainDatabase db)
        {
            var result = db.Trust.Add(trust);
            if (result < 1)
                return result;

            foreach (var subject in trust.Issuer.Subjects)
            {
                subject.IssuerId = trust.Issuer.Id;
                subject.TrustId = trust.TrustId;
                result = db.Subject.Add(subject);
                if (result < 1)
                    break;
            }
            return result;
        }


        public void EnsureTrustId(TrustModel trust, ITrustBinary trustBinary)
        {
            if (trust.TrustId != null && trust.TrustId.Length > 0)
                return;

            trust.TrustId = TrustECDSASignature.GetHashOfBinary(trustBinary.GetIssuerBinary());
        }

        public void VerifyTrust(TrustModel trust)
        {
            var schema = new TrustSchema(trust);
            if (!schema.Validate())
            {
                var msg = string.Join(". ", schema.Errors.ToArray());
                throw new ApplicationException(msg);
            }

            var signature = new TrustECDSASignature(trust);
            var errors = signature.VerifyTrustSignature();
            if (errors.Count > 0)
                throw new ApplicationException(string.Join(". ", errors.ToArray()));

        }
    }
}
