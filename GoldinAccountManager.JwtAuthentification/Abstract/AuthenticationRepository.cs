using GoldinAccountManager.JwtAuthentification.Interface;
using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoldinAccountManager.JwtAuthentification.Abstract
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationRepository> _logger;


        public AuthenticationRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthenticationRepository> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<AuthenticationModel> LoginAsync(LoginRequest model)
        {
            try
            {
                var auth = new AuthenticationModel();

                if (model != null)
                {
                    var user = await GetIdentityUserByUsernameAsync(model.Username);
                    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format("User with name {0} found on the database and successfully logged in.", model.Username)));

                        var userRoles = await _userManager.GetRolesAsync(user);

                        var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var token = GetToken(authClaims);

                        auth.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        auth.Expiration = token.ValidTo;
                        auth.Valid = true;

                        return auth;
                    }
                    else
                    {
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format("Failed to login user {0}.", model.Username)));
                    }
                }
                return auth;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }

        }
        public async Task<IdentityUser> GetIdentityUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format("Failed to find user {0}.", username)));
                    return null;
                }
                else
                {
                    _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format("Found user with username {0}.", username)));
                    return user;
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }
        public async Task<RegisterResponse> RegisterUserAsync(RegisterModel model)
        {
            try
            {
                var register = new RegisterResponse();
                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    register.Message = "User creation failed! Please check user details and try again.";
                    register.Valid = false;
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, register.Message));
                    return register;
                }
                else
                {
                    register.Message = "User created successfully!";
                    register.Valid = true;
                    _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, register.Message));
                    return register;
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                return new RegisterResponse { Valid = false, Message = ex.Message };
            }
        }

        public async Task<RegisterResponse> RegisterAdminAsync(RegisterModel model)
        {
            var register = new RegisterResponse();
            try
            {
                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    register.Message = "User creation failed! Please check user details and try again.";
                    register.Valid = false;
                    _logger.LogError(string.Format("{0} - {1}", DateTime.Now, register.Message));
                    return register;
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }
                register.Message = "User created successfully!";
                register.Valid = true;
                _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, register.Message));
                return register;

            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                return new RegisterResponse { Valid = false, Message = ex.Message };
            }

        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
