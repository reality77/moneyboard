using dal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("bridge")]
    public abstract class BridgeController : Controller
    {
        public BridgeController()
        {

        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            //TODO : appel de redirection de bridge api
            return Ok();
        }

        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            //TODO : retour de redirection de bridge api
            return Ok();
        }
    }
}
