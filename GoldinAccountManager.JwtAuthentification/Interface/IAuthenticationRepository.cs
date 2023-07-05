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
        /// <summary>
        /// Interface that exposes login for api user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationModel> LoginAsync(LoginRequest model);
        /// <summary>
        /// Interface that returns an api user given a username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<IdentityUser> GetIdentityUserByUsernameAsync(string username);
        /// <summary>
        /// Interface that register an api user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<RegisterResponse> RegisterUserAsync(RegisterModel model);
        /// <summary>
        /// Interface that registers an admin api user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<RegisterResponse> RegisterAdminAsync(RegisterModel model);
    }
}
