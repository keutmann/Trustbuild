using System;
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
  ""issuer"": {
      ""id"": ""z9+7gcKC3nbtUDd3r45Sr60nkS8="",
      ""signature"" : ""H+ybXlrtsk5Pm+TbccEky/3FR2bWWsBVUlI7RdZGicEPTI32pUFN30RGNiU9U3I1uXpMTSeKy8krgSfHdvxMnzA="",
      ""subject"": [{
          ""idtype"": ""person"", 
          ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"", 
          ""claim"": {
                ""trust"" : ""true"",
                ""confirm"" : ""true""
              },
          ""cost"": 100,
          ""activate"": 0, 
          ""expire"":   0,
          ""scope"": ""global"" 
        }
      ]
    }
  ,
  ""server"": {
    ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"",
    ""signature"" : ""H+ybXlrtsk5Pm+TbccEky/3FR2bWWsBVUlI7RdZGicEPTI32pUFN30RGNiU9U3I1uXpMTSeKy8krgSfHdvxMnzA="",
  },
  ""timestamp"": {
    ""btc"" : {
          ""hash"": ""SHA160"", 
          ""path"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
        },
    ""eth"" : {
          ""hash"": ""SHA3"", 
          ""path"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
        }
  }
}
";
        // Target "trustid": "fW32dXWOOp+P0rwslWs5H4gSpTkp6+anA9NECxkmZHo="
        public static string ClientJSON = @"
{
	""head"": {
		""version"": ""standard 0.1.0"",
		""script"": ""btc-pkh""
	},
	""issuer"": {
		""id"": ""RzUTJlQoHLzgtXjKiBx4+QAg1aQ="",
		""signature"": ""IP34/ye5vo+Wc4JrsseoLVLWy8kVorq7duVQgAYpF1+OYKWVXwySz1kDwbvO5ZthDlEmjAgB8KFNGPVK00K/5Zc="",
		""subject"": [{
			""id"": ""RzUTJlQoHLzgtXjKiBx4+QAg1aQ="",
			""idtype"": ""identity"",
			""scope"": ""reddit"",
			""claim"": {
				""trust"": ""true""
			},
			""cost"": 100,
			""activate"": 0,
			""expire"": 0
		}, {
			""id"": ""K2c618oiqO547JJ9bWs6lsKFWCI="",
			""idtype"": ""name"",
			""scope"": ""reddit"",
			""claim"": {
				""trust"": ""true""
			},
			""cost"": 100,
			""activate"": 0,
			""expire"": 0
		}]
	},
	
}
";

    }
}
