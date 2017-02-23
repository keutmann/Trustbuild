using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Model;

namespace TrustchainCore.Business
{
    public class TrustSchema
    {

        public List<string> Errors { get; set; }

        protected TrustModel trust { get; set; }

        public TrustSchema(TrustModel t)
        {
            trust = t;
            Errors = new List<string>();
        }


        public bool Validate()
        {
            if (trust.Issuer == null)
                Errors.Add("Missing Issuer");

            if (trust.Issuer.Id == null || trust.Issuer.Id.Length == 0)
                Errors.Add("Missing issuer id");

            //if (trust.Issuer.Signature == null)
            //    Errors.Add("Missing issuer signature");

            if (trust.Issuer.Subjects == null || trust.Issuer.Subjects.Length == 0)
                Errors.Add("Missing subject");

            var index = 0;
            foreach (var subject in trust.Issuer.Subjects)
            {
                if (subject.Id == null || subject.Id.Length == 0)
                    Errors.Add("Missing subject id at index: "+index);
                index++;
            }

            return Errors.Count == 0;
        }
    }
}
