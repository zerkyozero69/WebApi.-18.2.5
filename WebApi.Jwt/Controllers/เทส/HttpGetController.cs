using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Jwt.Controllers.เทส
{
    public class HttpGetController : ApiController
    {
        [AllowAnonymous]
        [Route("route/test")]
        [HttpGet]
        public ContentResult Index()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><body>Hello World</body></html>"
            };
        }
    }
}
