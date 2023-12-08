using FavoriteBooks.API.Business.Services.Contracts;
using FavoriteBooks.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FavoriteBooks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet(Name = "Login")]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] RegisterModel model)
        {
            return Ok(new ResponseBase(await _authService.RegisterAsync(model)));
        }
    }
}
