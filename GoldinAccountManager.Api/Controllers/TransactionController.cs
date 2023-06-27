using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldinAccountManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
             
        // POST api/<TransactionController>
        [Route("CreditAccountByCard")]
        [HttpPost]
        public void Post([FromBody] CrebitByCardRequest value)
        {
        }

        [Route("CreditAccountByBank")]
        [HttpPost]
        public void Post([FromBody] BankEFTRequest value)
        {
        }
        [Route("DebitAccount")]
        [HttpPost]
        public void Post([FromBody] DebitRequest value)
        {
        }

        [Route("GetAccountStatement")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
