using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PropietarioModel = InmobiliariaApi.Features.Propietario.Propietario;

namespace InmobiliariaApi.Features.Auth
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly InmobiliariaDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<PropietarioModel> _passwordHasher;

    public AuthController(InmobiliariaDbContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
      _passwordHasher = new PasswordHasher<PropietarioModel>();
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<SimpleLoginResponseDto>> Login(LoginRequestDto loginRequest)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // Buscar propietario por email
      var propietario = await _context.Propietarios
        .FirstOrDefaultAsync(p => p.Mail == loginRequest.Email);

      if (propietario == null)
      {
        return Unauthorized(new SimpleLoginResponseDto
        {
          Success = false,
          Message = "Email o contraseña incorrectos"
        });
      }

      // Verificar contraseña
      var result = _passwordHasher.VerifyHashedPassword(propietario,
        propietario.Password, loginRequest.Password);

      if (result == PasswordVerificationResult.Failed)
      {
        return Unauthorized(new SimpleLoginResponseDto
        {
          Success = false,
          Message = "Email o contraseña incorrectos"
        });
      }

      // Generar JWT
      var token = GenerateJwtToken(propietario.IdPropietario);

      return Ok(new SimpleLoginResponseDto
      {
        Success = true,
        Message = "Login exitoso",
        Token = token
      });
    }

    // GET: api/auth/me
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<PropietarioAuthDto>> GetCurrentUser()
    {
      // Obtener el ID del propietario desde el JWT
      var idPropietarioString = User.FindFirst("idPropietario")?.Value;

      if (string.IsNullOrEmpty(idPropietarioString) || !int.TryParse(idPropietarioString, out int idPropietario))
      {
        return Unauthorized(new { message = "Token inválido" });
      }

      // Buscar el propietario
      var propietario = await _context.Propietarios
        .FirstOrDefaultAsync(p => p.IdPropietario == idPropietario);

      if (propietario == null)
      {
        return NotFound(new { message = "Propietario no encontrado" });
      }

      return Ok(new PropietarioAuthDto
      {
        IdPropietario = propietario.IdPropietario,
        Dni = propietario.Dni,
        Apellido = propietario.Apellido,
        Nombre = propietario.Nombre,
        Mail = propietario.Mail,
        Telefono = propietario.Telefono
      });
    }

    // Generar JWT
    private string GenerateJwtToken(int idPropietario)
    {
      // Leer configuración appsettings.json
      var secretKey = _configuration["Jwt:SecretKey"];
      var issuer = _configuration["Jwt:Issuer"];
      var audience = _configuration["Jwt:Audience"];
      var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

      // Crear los claims
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, idPropietario.ToString()),
        new Claim("idPropietario", idPropietario.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
      };

      // Crear el JWT
      var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}