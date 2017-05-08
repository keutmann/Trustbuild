using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using TrustbuildCore.Business;

namespace TrustbuildCore.Controllers
{
    public class TrustController : ApiController
    {
        public const string Path = "/api/trust/";

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("OK");
        }

        [HttpPost]
        public IHttpActionResult Add(HttpRequestMessage requrest)
        {
            try
            {
                var id = requrest.Content.ReadAsStringAsync().Result;
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
