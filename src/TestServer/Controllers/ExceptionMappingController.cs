using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyLab.WebErrors;

namespace TestServer.Controllers
{
    [Route("api/exception-mapping")]
    [ApiController]
    public class ExceptionMappingController : ControllerBase
    {
        [HttpGet("with-message")]
        [ErrorToResponse(typeof(NullReferenceException), HttpStatusCode.NotFound, "foo")]
        public ActionResult<IEnumerable<string>> GetWithMessage()
        {
            throw new NullReferenceException("bar");
        }

        [HttpGet("without-message")]
        [ErrorToResponse(typeof(NullReferenceException), HttpStatusCode.NotFound)]
        public ActionResult<IEnumerable<string>> GetWithoutMessage()
        {
            throw new NullReferenceException("bar");
        }
    }
}
