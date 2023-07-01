using GoldinAccountManager.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.JwtAuthentification.Interface
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticationModel> LoginAsync(LoginRequest model);
        Task<IdentityUser> GetIdentityUserByUsernameAsync(string username);
        Task<RegisterResponse> RegisterUserAsync(RegisterModel model);
        Task<RegisterResponse> RegisterAdminAsync(RegisterModel model);
    }
}
