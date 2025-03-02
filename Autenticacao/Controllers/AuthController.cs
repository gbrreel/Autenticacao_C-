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
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userId = await _authService.RegisterAsync(request.Email, request.Password);
            if (userId == null)
                return BadRequest(new { message = "Erro ao registrar usuário no Supabase." });

            return Ok(new { UserId = userId });
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token == null)
                return Unauthorized(new { message = "Credenciais inválidas!" });

            return Ok(new { Token = token });
        }

        public class RegisterRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // FORMATO DO JSON PARA LOGIN 
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
};

