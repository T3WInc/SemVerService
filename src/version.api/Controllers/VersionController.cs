using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using t3winc.version.common.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace t3winc.version.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionRepo _repo;
        private readonly IProductRepo _prodRepo;

        public VersionController(IVersionRepo repo, IProductRepo prodrepo)
        {
            _repo = repo;
            _prodRepo = prodrepo;
        }

        // POST api/<VersionController>
        /// <summary>
        /// This api takes in the name of an organization,
        /// checks to confirm it does not already exist 
        /// and returns a new key that you will need for
        /// any other api call that it made.
        /// </summary>
        /// <param name="organization">The Name of the Organization</param>
        /// <returns>An Api Key to be used for all other calls.</returns>
        [HttpPost]
        public IActionResult Post(string organization)
        {
            string result = _repo.NewRegistration(organization);
            if (result == "Sorry, This Organizaition already exits")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<VersionController>
        /// <summary>
        /// This api takes in the organization key, product name and branch and will return the 
        /// the next version number for this product. It goes through a number of checks like
        /// checking if this is a new branch or an existing branch and makes the appropriate
        /// increments and then returns the resulting value.
        /// </summary>
        /// <param name="key">The Api key returned from the Post call.</param>
        /// <param name="product">The product that has been setup for this organization.  Product must already exist.</param>
        /// <param name="branch">The branch that the code is being commited from this can be a new or existing branch.</param>
        /// <returns>The next version number for the product.</returns>
        public IActionResult Get(string key, [FromQuery(Name = "Product")] string product, [FromQuery(Name = "Branch")] string branch)
        {
            var version = _repo.GetVersionId(key);
            if (_repo.IsKeyValid(key) && _prodRepo.ProductExist(version, product))
            {
                var result = _repo.GetNextVersionNumber(version, product, branch);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}