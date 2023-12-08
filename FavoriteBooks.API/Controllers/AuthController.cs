using FavoriteBooks.API.Business.Services.Contracts;
using FavoriteBooks.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FavoriteBooks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            return Ok(new ResponseBase(await authService.RegisterAsync(model)));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            return Ok(new ResponseBase(await authService.LoginAsync(model)));
        }
    }
}
