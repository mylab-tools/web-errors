using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestServer.Controllers
{
    [Route("api/exception-hiding")]
    [ApiController]
    public class ExceptionHidingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            throw new NullReferenceException("bar");
        }
    }
}