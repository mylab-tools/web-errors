using System;
using Microsoft.AspNetCore.Mvc;

namespace TestServerCore31.Controllers
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