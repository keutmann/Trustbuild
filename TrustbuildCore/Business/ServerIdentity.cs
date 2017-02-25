using NBitcoin;
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


        private ServerIdentity() : this(App.Config["serverwif"].ToString(), App.BitcoinNetwork)
        {

        }

        public ServerIdentity(string wif, Network network)
        {
            PrivateKey = Key.Parse(wif, network);
            Address = PrivateKey.PubKey.GetAddress(App.BitcoinNetwork);
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
