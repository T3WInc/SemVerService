using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t3winc.version.common.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace t3winc.version.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IVersionRepo _verepo;
        private readonly IProductRepo _repo;
        public ProductController(IProductRepo repo, IVersionRepo verepo)
        {
            _verepo = verepo;
            _repo = repo;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductController>/5
        [HttpGet("{key}")]
        public IActionResult Get(string key, [FromQuery(Name = "Product")] string product)
        {
            if (_verepo.IsKeyValid(key))
            {
                var result = _repo.GetProduct(product);
                return Ok(result);
            }
            return BadRequest();
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post(string key, [FromQuery(Name = "Product")] string product)
        {
            if (_verepo.IsKeyValid(key))
            {
                int versionId = _verepo.GetVersionId(key);
                string result = _repo.NewProduct(versionId, product);
                return Ok(result);
            }
            return BadRequest();
        }

        // PUT api/<ProductController>/5
        [HttpPut("{key}")]
        public IActionResult Put(string key, [FromQuery(Name = "Product")] string product, [FromQuery(Name = "Increment")] string increment)
        {
            if (_verepo.IsKeyValid(key))
            {
                switch (increment)
                {
                    case "Major":
                        _repo.IncrementMajor(product);
                        return Ok();
                    case "Minor":
                        _repo.IncrementMinor(product);
                        return Ok();
                    case "Patch":
                        _repo.IncrementPatch(product);
                        return Ok();
                    default:
                        return BadRequest();
                }
            }
            return BadRequest();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
