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
        private readonly ITransactionRepository _transaction;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IAccountRepository account, ITransactionRepository transaction, ILogger<TransactionController> logger)
        {
            _transaction = transaction;
            _logger = logger;
        }

        /// <summary>
        /// This credits an account via credit cards or debit cards.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns a newly added transaction.</returns>
        [Route("CreditAccountByCard")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AmountShouldBeGreaterThanZero));
                        return BadRequest(ApplicationMessages.AmountShouldBeGreaterThanZero);
                    }
                }
                else
                {
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.CardDetailsEntry));
                    return BadRequest(ApplicationMessages.CardDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This credits an account via bank accoun.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns a newly added transaction.</returns>
        [Route("CreditAccountByBank")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] BankEFTRequest value)
        {
            try
            {
                if (value != null)
                {
                    if (value.Amount > 0)
                    {
                        var creditAccount = await _transaction.CreditAccountByBankAsync(value);
                        return CreatedAtAction(nameof(Post), new { id = creditAccount }, creditAccount);
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AmountShouldBeGreaterThanZero));
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.AmountShouldBeGreaterThanZero }); ;

                    }
                }
                else
                {
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.BankingDetailsEntry));
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.BankingDetailsEntry }); ;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This debits an account with the entered amount, the account must have a balance greater than zero.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns a newly added transaction.</returns>
        [Route("DebitAccount")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] DebitRequest value)
        {
            try
            {
                if (value != null)
                {
                    if (value.Amount > 0)
                    {
                        var debitAccount = await _transaction.DebitAccountAsync(value);
                        return CreatedAtAction(nameof(Post), new { id = debitAccount }, debitAccount);
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AmountShouldBeGreaterThanZero));
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.AmountShouldBeGreaterThanZero }); ;

                    }
                }
                else
                {
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.DebitDetailsEntry));
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.DebitDetailsEntry }); ;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This returns an account statement based on a given date range.
        /// </summary>
        /// <param name="accountStatementRequest"></param>
        /// <returns> Returns an account statement with a total balance based on a gate range.</returns>
        [Route("GetAccountStatement")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Get([FromQuery] AccountStatementRequest accountStatementRequest)
        {
            try
            {
                    return Ok(await _transaction.GetAccountStatementAsync(accountStatementRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This get all transactions in the database.
        /// </summary>
        /// <returns> Returns all transactions on the database via MS SQL or Redis cache.</returns>
        [HttpGet]
        [Route("GetTransactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Transaction>? trans;
                trans = await _transaction.GetAllTransactionsAsync();
                if (trans?.Count > 0)
                    return Ok(trans);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccountTransactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int value)
        {
            try
            {
                List<Transaction>? trans;
                trans = await _transaction.GetAccountTransactionsAsync(value);
                if (trans?.Count > 0)
                    return Ok(trans);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }

    }
}
