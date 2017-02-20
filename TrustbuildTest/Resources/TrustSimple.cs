﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustbuildTest.Resources
{
    public class TrustSimple
    {
        public static string JSON = @"
  {
    ""head"": {
    ""version"": ""standard 0.1.0"", 
    ""script"": ""btc-pkh"" 
  },
  ""issuer"": 
    {
      ""id"": ""z9+7gcKC3nbtUDd3r45Sr60nkS8="",
      ""subject"": [
        {
          ""index"": 0,
          ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"", 
          ""idtype"": ""person"", 
          ""claimtype"": ""simpletrust"", 
          ""claim"": [{
            ""type"": ""trust"", 
            ""data"": ""true""
              },
              {
                ""type"": ""confirm"", 
                ""data"": ""true""
              }],
          ""cost"": 100,
          ""activate"": ""2017-02-18T23:44:41.7620108+01:00"", 
          ""expire"":   ""2017-02-18T23:44:41+01:00"",
          ""scope"": ""global"" 
        }
      ]
    }
  ,
  ""server"": {
    ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
  },
  ""timestamp"": [
    {
      ""blockchain"": ""bitcoin"",
      ""hash"": ""SHA160"", 
      ""path"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
    },
    {
      ""blockchain"": ""ethereum"",
      ""hash"": ""SHA3"", 
      ""path"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
    }
  ],
  ""signature"": {
    ""server"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"",
    ""issuer"": ""H+ybXlrtsk5Pm+TbccEky/3FR2bWWsBVUlI7RdZGicEPTI32pUFN30RGNiU9U3I1uXpMTSeKy8krgSfHdvxMnzA="",
    ""subject"": [
        {
        ""index"": 0,
        ""proof"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
        }
    ]
  }
}
";
    }
}