using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _account.GetAllAccountsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        // GET: api/<AccountController>/1234567890123
        [HttpGet]
        [Route("GetAccountById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string value)
        {
            try
            {
                return Ok(await _account.GetAccountByIdAsync(value));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        // POST api/<AccountController>
        [Route("AddAccount")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AccountRequest value)
        {
            try
            {
                if (value != null)
                {
                    var addAccount = await _account.AddAccountAsync(value);
                    return CreatedAtAction(nameof(Post), new { id = addAccount }, addAccount);
                }
                else { return BadRequest($"Please enter account details."); }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<AccountController>/5
        [HttpPut]
        [Route("UpdateAccount")]
        public void Put([FromBody] Account value)
        {
        }

       
    }
}
