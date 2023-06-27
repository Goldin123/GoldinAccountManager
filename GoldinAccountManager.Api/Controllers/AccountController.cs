using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldinAccountManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _account;

        public AccountController(IAccountRepository account) 
        {
            _account = account;
        }
        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [Route("AddAccount")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account value)
        {
            var acc = new Account
            {
                AccountID = 1,
                FirstName = "Test1",
                LastName = "LastTest"
            };

            var a = await _account.AddAccountAsync(acc);
            return CreatedAtAction(nameof(Post), new { id = acc.AccountID }, acc);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
