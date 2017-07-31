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
    public class TrustBuildManager : TrustManager
    {
        public DateTime Timestamp = DateTime.Now;

        public TrustModel AddNew(string content)
        {
            var trust = JsonConvert.DeserializeObject<TrustModel>(content);
            return AddNew(trust);
        }

        public TrustModel AddNew(TrustModel trust)
        {
            EnsureTrustId(trust, new TrustBinary(trust));

            VerifyTrust(trust);

            // This is the servers task for later processing
            trust.Timestamp = null; 

            AddToDatabase(trust);

            return trust;
        }

        
        public void AddToDatabase(TrustModel trust)
        {
            

            trust.DatabaseName = GetCurrentDBTrustname();

            var fullpath = Path.Combine(AppDirectory.BuildPath, trust.DatabaseName);

            using (var db = TrustchainDatabase.Open(fullpath))
            {
                db.CreateIfNotExist();

                var item = db.Trust.SelectOne(trust.TrustId);
                if (item != null)
                    return; // The trust already exist in the db.

                AddTrust(trust, db);
            }
        }

        public static void EnsureServerId(TrustModel trust)
        {
            if (trust.Server != null && trust.Server.Id.Length > 0)
                return;

            trust.Server = new ServerModel();
            trust.Server.Id = ServerIdentity.Current.Address.Hash.ToBytes();
        }


        public string GetCurrentDBTrustname()
        {
            var preName = ServerIdentity.Current.Address.ToWif();
            return string.Format("{0}_{1}.db", preName, DefaultPartition());
        }

        public string DefaultPartition()
        {
            return GetPartition(Timestamp);
        }

        public string GetPartition(DateTime datetime)
        {
            return datetime.ToString(App.Config["partition"].ToStringValue("yyyyMMddhh0000"));
        }


    }
}
