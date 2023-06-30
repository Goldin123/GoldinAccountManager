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
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IAccountRepository _account;

        public AccountController(IAccountRepository account, ILogger<AccountController> logger)
        {
            _account = account;
            _logger = logger;
        }
        // GET: api/<AccountController>
        [HttpGet]
        [Route("GetAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var accounts = await _account.GetAllAccountsAsync();
                if (accounts?.Count > 0)
                    return Ok(await _account.GetAllAccountsAsync());
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        // GET: api/<AccountController>/1234567890123
        [HttpGet]
        [Route("GetAccountById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(string value)
        {
            try
            {
                return Ok(await _account.GetAccountByIdAsync(value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        // POST api/<AccountController>
        [Route("AddAccount")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody] AccountRequest value)
        {
            try
            {
                if (value != null)
                {
                    var addAccount = await _account.AddAccountAsync(value);
                    return CreatedAtAction(nameof(Post), new { id = addAccount }, addAccount);
                }
                else 
                {
                    _logger.LogError(ApplicationMessages.AccountDetailsEntry);
                    return BadRequest(ApplicationMessages.AccountDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [Route("AddAccounts")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody] List<AccountRequest> values)
        {
            try
            {
                if (values != null)
                {
                    var addAccounts = await _account.AddAccountsAsync(values);
                    return CreatedAtAction(nameof(Post), new { id = addAccounts }, addAccounts);
                }
                else
                {
                    _logger.LogError(ApplicationMessages.AccountDetailsEntry);
                    return BadRequest(ApplicationMessages.AccountDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<AccountController>/5
        [HttpPut]
        [Route("UpdateAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Put([FromBody] AccountRequest value)
        {
            try
            {
                if (value != null)
                {
                    var updateAccount = await _account.UpdateAccountAsync(value);
                    return CreatedAtAction(nameof(Post), new { id = updateAccount }, updateAccount);
                }
                else 
                {
                    _logger.LogError(ApplicationMessages.AccountDetailsEntry);
                    return BadRequest(ApplicationMessages.AccountDetailsEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }


    }
}
