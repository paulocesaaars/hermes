using Deviot.Common;
using Deviot.Hermes.Api.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deviot.Hermes.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/health-check")]
    public class HeathCheckController : CustomControllerBase
    {

        public HeathCheckController(INotifier notifier, 
                                    ILogger<HeathCheckController> logger) : base(notifier, logger)
        {
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult Get() => Ok();
    }
}
