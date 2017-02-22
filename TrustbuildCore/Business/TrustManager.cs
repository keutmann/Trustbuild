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
            var trust = JsonConvert.DeserializeObject<Trust>(content);

            VerifyTrust(trust);

            SignServerSignature(trust);

            // This is the servers task for later processing
            trust.Timestamp = null; 

            AddToDatabase(trust);
        }

        public void SignServerSignature(Trust trust)
        {
            var binary = new TrustBinary(trust);

            var trustHash = TrustECDSASignature.GetHashOfBinary(binary.GetIssuerBinary());

            var serverkey32 = Key.Parse(App.Config["serverwif"].ToString(), App.BitcoinNetwork);

            trust.Server = new Server();
            trust.Server.Id = serverkey32.PubKey.GetAddress(App.BitcoinNetwork).Hash.ToBytes();
            trust.Server.Signature = serverkey32.SignCompact(trustHash);
        }

        public void AddToDatabase(Trust trust)
        {
            var dbname = GetCurrentDBTrustname(trust.Server.Id);
            var fullpath = Path.Combine(App.Config["buildpath"].ToStringValue(), dbname);

            using (var db = TrustchainDatabase.Open(fullpath))
            {
                db.CreateIfNotExist();

                var items = db.Trust.Select(trust.Issuer.Id, trust.Issuer.Signature);
                if (items.Count() > 0)
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



        public int AddTrust(Trust trust, TrustchainDatabase db)
        {
            var result = db.Trust.Add(trust);
            if (result < 1)
                return result;

            foreach (var subject in trust.Issuer.Subjects)
            {
                subject.IssuerId = trust.Issuer.Id;
                result = db.Subject.Add(subject);
                if (result < 1)
                    break;
            }
            return result;
        }


        public void VerifyTrust(Trust trust)
        {
            var schema = new TrustSchema(trust);
            if (!schema.Validate())
            {
                var msg = string.Join(". ", schema.Errors.ToArray());
                throw new ApplicationException(msg);
            }

            var binary = new TrustBinary(trust);

            var signature = new TrustECDSASignature(trust, binary);
            var errors = signature.VerifyTrustSignature();
            if (errors.Count > 0)
                throw new ApplicationException(string.Join(". ", errors.ToArray()));

        }
    }
}
