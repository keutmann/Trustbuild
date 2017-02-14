using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace TrustbuildCore.Controllers
{
    public class TrustController : ApiController
    {
        public const string Path = "/api/proof/";

        [HttpPost]
        public IHttpActionResult Add([FromUri]string id)
        {
            try
            {
                return Ok("");
            }
            catch (Exception ex)
            {
                return new ExceptionResult(ex, this);
            }
        }
    }
}
