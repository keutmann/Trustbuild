using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Model;

namespace TrustchainCore.Data
{
    public class TrustSignature
    {
        protected Trust trust { get; set; }

        public TrustSignature(Trust t)
        {
            trust = t;
        }


        public void SignIssuer(byte[] privatekey)
        {

        }
    }
}
