using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Auth.Jwt;
using Microsoft.AspNetCore.Http;

namespace Auth.Controllers
{
    public class PolicyController : Controller
    {
        [Authorize(Policy = "MaleAbove21")]
        public IActionResult MaleAbove21()
        {
            return Ok();
        }

        [Authorize(Policy = "nonAdminUser")]
        public IActionResult NonAdminUser()
        {
            return Ok();
        }
        [Authorize(Policy = "both")]
        public IActionResult Both()
        {
            return Ok();
        }
    }
}