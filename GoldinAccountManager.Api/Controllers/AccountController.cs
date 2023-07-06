using GoldinAccountManager.API.Helper;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
        private readonly IDistributedCache _cache;
        private readonly string _accountsRedisrecordKey = $"Accounts_{DateTime.Now:yyyyMMdd_hh}";
        private string _fetchLoadLocation = "";

        public AccountController(IAccountRepository account, ILogger<AccountController> logger, IDistributedCache cache)
        {
            _account = account;
            _logger = logger;
            _cache = cache;
        }
        /// <summary>
        /// This gets all the accounts created on the database.
        /// </summary>
        /// <returns>If founds, returns all the accounts from MS SQL or Redis cache.</returns>
        [HttpGet]
        [Route("GetAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Account>? accounts;
                accounts = await _account.GetAllAccountsAsync();
                if (accounts?.Count > 0)
                    return Ok(accounts);
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
        /// This gets account details by linked to an AccountID, 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns account details that matches to the AccountID.</returns>
        [HttpGet]
        [Route("GetAccountById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int value)
        {
            try
            {
                return Ok(await _account.GetAccountByIdAsync(value));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This add a single account to the database.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns the recently added account.</returns>
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
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountDetailsEntry));
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.AccountDetailsEntry }); ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This add a collection of accounts to the database.
        /// </summary>
        /// <param name="values"></param>
        /// <returns>Returns the newly created list of accounts.</returns>
        [Route("AddAccounts")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountDetailsEntry));
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.AccountDetailsEntry }); ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This updates an account based on a matching identity number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns the newly updated account details.</returns>
        [HttpPut]
        [Route("UpdateAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountDetailsEntry));
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ApplicationMessages.AccountDetailsEntry }); ;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ex.ToString()));
                return BadRequest(ex.Message);
            }
        }
    }
}
