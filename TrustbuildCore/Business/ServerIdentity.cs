
using NBitcoin;
using NBitcoin.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustbuildCore.Service;

namespace TrustbuildCore.Business
{
    public class ServerIdentity
    {
        public Key PrivateKey { get; set; }
        public BitcoinPubKeyAddress Address { get; set; }


        //private ServerIdentity() : this(App.Config["serverwif"].ToString(), App.BitcoinNetwork)
        private ServerIdentity() : this("", App.BitcoinNetwork)
        {

        }

        public ServerIdentity(string wif, Network network)
        {
            Key key = null;
            if (string.IsNullOrEmpty(wif))
            {
                key = new Key(Hashes.Hash256(Encoding.UTF8.GetBytes(Environment.MachineName)).ToBytes());
                App.Config["serverwif"] = key.GetBitcoinSecret(network).ToWif();
            }
            else
            {
                key = Key.Parse(wif, network);
            }
            Address = key.PubKey.GetAddress(App.BitcoinNetwork);
        }

        public static ServerIdentity Current { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly ServerIdentity instance = new ServerIdentity();
        }
    }
}
