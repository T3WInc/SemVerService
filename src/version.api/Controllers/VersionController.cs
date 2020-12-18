using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t3winc.version.common.Interfaces;
using t3winc.version.data.repos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace t3winc.version.api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionRepo _repo;

        public VersionController(IVersionRepo repo)
        {
            _repo = repo;
        }

        // GET: api/<VersionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<VersionController>
        [HttpPost]
        public IActionResult Post(string value)
        {
            string result =  _repo.NewRegistration(value);
            if(result == "Sorry, This Organizaition already exits")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
