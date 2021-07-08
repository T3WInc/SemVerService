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

        // GET api/<ProductController>/5
        /// <summary>
        /// This Api call will return the status of the Product. This includes
        /// the current version numbers as it pertains to the master branch.
        /// </summary>
        /// <param name="key">The Api Key from the Version Post call</param>
        /// <param name="product">The Name of the Product</param>
        /// <returns>Current Product Master Branch Version Numbers</returns>
        [HttpGet("{key}/product")]
        public IActionResult Get(string key, [FromQuery(Name = "Product")] string product)
        {
            var version = _verepo.GetVersionId(key);
            if (_verepo.IsKeyValid(key) && _repo.ProductExist(version, product))
            {
                var result = _repo.GetProduct(version, product);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            var version = _verepo.GetVersionId(key);
            if (_verepo.IsKeyValid(key))
            {
                var result = _repo.GetAllProducts(version);
                return Ok(result);
            }
            return BadRequest();
        }

        // POST api/<ProductController>
        /// <summary>
        /// This Api call will create a new Product if it does not already exist for
        /// the Organzation determined from the api key.
        /// </summary>
        /// <param name="key">The Api Key from the Version Post call</param>
        /// <param name="product">The name of the Product should match the git repo name</param>
        /// <returns>Returns the current version number of the Master Branch.</returns>
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
        /// <summary>
        /// This Api will increment a version number, which part of the version number will
        /// depend on the value passed into the Increment parameter.
        /// </summary>
        /// <param name="key">The Api Key from the Version Post call</param>
        /// <param name="product">The name of the Product which should match the git repo name</param>
        /// <param name="increment">Major, Minor or Patch anything else is invalid</param>
        /// <returns>Code 200 if sucessfull</returns>
        [HttpPut]
        public IActionResult Put(string key, [FromQuery(Name = "Product")] string product, [FromQuery(Name = "Increment")] string increment)
        {
            if (_verepo.IsKeyValid(key))
            {
                var versionId = _verepo.GetVersionId(key);
                if (_repo.ProductExist(versionId, product))
                {
                    switch (increment)
                    {
                        case "Major":
                            _repo.IncrementMajor(versionId, product);
                            return Ok();
                        case "Minor":
                            _repo.IncrementMinor(versionId, product);
                            return Ok();
                        case "Patch":
                            _repo.IncrementPatch(versionId, product);
                            return Ok();
                        default:
                            return BadRequest();
                    }
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
