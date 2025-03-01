using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Autenticacao.Services;
using Microsoft.AspNetCore.Identity.Data;

namespace Autenticacao.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SupabaseAuthService _authService;

        public AuthController(SupabaseAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token == null)
                return Unauthorized(new { message = "Credenciais invalidas!" });

            return Ok(new { Token = token });
        }
    }
};

