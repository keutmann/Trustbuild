using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using TrustbuildCore.Business;
using TrustchainCore.Model;

namespace TrustbuildCore.Controllers
{
    public class TrustController : ApiController
    {
        public const string Path = "/api/trust/";

        private ITrustBuildManager buildManager;

        public TrustController()
        {
            buildManager = new TrustBuildManager();
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new { status = "succes" });
        }

        [HttpPost]
        public IHttpActionResult Add([FromBody]PackageModel package)
        {
            try
            {
                buildManager.Add(package);

                return Ok(new { status= "succes" });
            }
            catch (Exception ex)
            {
                return new ExceptionResult(ex, this);
            }
        }
    }
}
