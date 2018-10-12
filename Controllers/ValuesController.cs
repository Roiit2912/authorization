using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sample;

namespace temp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            User u = new UserRepository().GetUser(user.Username);
  	     if (u == null)
                  return BadRequest("The User does not exist.");

              bool credentials = u.Password.Equals(user.Password);
              if (!credentials) 
              {return  BadRequest("The username/password combination was wrong.");}

              else{
                  var token = TokenManager.GenerateToken(user.Username);

                  return Ok(token); 
                   

              }
              

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
