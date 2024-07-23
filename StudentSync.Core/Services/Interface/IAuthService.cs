using StudentSync.Core.Wrapper;
using StudentSync.Data.Models;
using StudentSync.Data.ViewModels;

namespace StudentSync.Core.Services.Interface
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(RegisterViewModel model);
        Task<IResult> LoginAsync(LoginViewModel model);
        Task<IResult> LogoutAsync();
        Task<IResult> AdminLoginAsync(LoginViewModel model); // New method for admin login
                                                             

    }
}
