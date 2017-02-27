using System;
using System.Web.Http;
using System.Web.Http.Results;
using TrustbuildCore.Business;

namespace TrustbuildCore.Controllers
{
    public class TrustController : ApiController
    {
        public const string Path = "/api/trust/";

        [HttpPost]
        public IHttpActionResult Add([FromBody]string id)
        {
            try
            {
                var manager = new TrustBuildManager();

                manager.AddNew(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return new ExceptionResult(ex, this);
            }
        }
    }
}
