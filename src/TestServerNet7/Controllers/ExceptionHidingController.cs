using Microsoft.AspNetCore.Mvc;

namespace TestServerNet7.Controllers
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