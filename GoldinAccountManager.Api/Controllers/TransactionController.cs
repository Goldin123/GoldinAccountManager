using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldinAccountManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IAccountRepository _account;
        private readonly ITransactionRepository _transaction;

        public TransactionController(IAccountRepository account,ITransactionRepository transaction) 
        {
            _account = account;
            _transaction = transaction;
        }

        // POST api/<TransactionController>
        [Route("CreditAccountByCard")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CrebitByCardRequest value)
        {
            try 
            {
                if (value != null)
                {
                    if (value.Amount > 0)
                    {
                        var addTransaction = await _transaction.CreditAccountByCardAsync(value);
                        return CreatedAtAction(nameof(Post), new { id = addTransaction }, addTransaction);
                    }
                    else 
                    {
                        return BadRequest($"Amount should be grater than zero.");
                    }

                }
                else { return BadRequest($"Please enter credit card details."); }

            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
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
        public IEnumerable<string> Get(int accountId,DateTime dateFrom,DateTime dateTo)
        {
            return new string[] { "value1", "value2" };
        }

    }
}
