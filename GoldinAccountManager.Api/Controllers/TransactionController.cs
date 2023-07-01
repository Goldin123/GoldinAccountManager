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
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IAccountRepository account, ITransactionRepository transaction, ILogger<TransactionController> logger)
        {
            _account = account;
            _transaction = transaction;
            _logger = logger;
        }

        // POST api/<TransactionController>
        [Route("CreditAccountByCard")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

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
                        _logger.LogError(ApplicationMessages.AmountShouldBeGreaterThanZero);
                        return BadRequest(ApplicationMessages.AmountShouldBeGreaterThanZero);
                    }
                }
                else
                {
                    _logger.LogError(ApplicationMessages.CardDetailsEntry);
                    return BadRequest(ApplicationMessages.CardDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [Route("CreditAccountByBank")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

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
                        _logger.LogError(ApplicationMessages.AmountShouldBeGreaterThanZero);
                        return BadRequest(ApplicationMessages.AmountShouldBeGreaterThanZero);
                    }
                }
                else
                {
                    _logger.LogError(ApplicationMessages.BankingDetailsEntry);
                    return BadRequest(ApplicationMessages.BankingDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [Route("DebitAccount")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

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
                        _logger.LogError(ApplicationMessages.AmountShouldBeGreaterThanZero);
                        return BadRequest(ApplicationMessages.AmountShouldBeGreaterThanZero);
                    }
                }
                else
                {
                    _logger.LogError(ApplicationMessages.DebitDetailsEntry);
                    return BadRequest(ApplicationMessages.DebitDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

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
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

    }
}
