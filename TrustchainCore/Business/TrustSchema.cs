﻿using System;
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

        protected Trust trust { get; set; }

        public TrustSchema(Trust t)
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

            if (trust.Issuer.Subjects == null || trust.Issuer.Subjects.Length == 0)
                Errors.Add("Missing subject");

            var index = 0;
            foreach (var subject in trust.Issuer.Subjects)
            {
                if (subject.Id == null || subject.Id.Length == 0)
                    Errors.Add("Missing subject id at index: "+index);
                index++;
            }

            if (trust.Signature == null)
                Errors.Add("Missing signature");

            if (trust.Signature.Issuer == null || trust.Signature.Issuer.Length == 0)
                Errors.Add("Missing issuer signature");

            return Errors.Count == 0;
        }
    }
}