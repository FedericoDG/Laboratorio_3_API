using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace InmobiliariaApi.Features.Propietario
{
  [Route("api/propietarios")]
  [ApiController]
  public class PropietarioController : ControllerBase
  {
    private readonly InmobiliariaDbContext _context;
    private readonly PasswordHasher<Propietario> _passwordHasher;

    public PropietarioController(InmobiliariaDbContext context)
    {
      _context = context;
      _passwordHasher = new PasswordHasher<Propietario>();
    }

    // GET: api/propietarios/me
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<PropietarioResponseDto>> GetMyProfile()
    {
      var idPropietario = int.Parse(User.FindFirst("idPropietario")?.Value ?? "0");

      var propietario = await _context.Propietarios
        .FirstOrDefaultAsync(p => p.IdPropietario == idPropietario);

      if (propietario == null)
      {
        return NotFound("Propietario no encontrado");
      }

      var response = new PropietarioResponseDto
      {
        IdPropietario = propietario.IdPropietario,
        Dni = propietario.Dni,
        Apellido = propietario.Apellido,
        Nombre = propietario.Nombre,
        Mail = propietario.Mail,
        Telefono = propietario.Telefono
      };

      return Ok(response);
    }

    // PUT: api/propietarios/me
    [HttpPut("me")]
    [Authorize]
    public async Task<ActionResult<PropietarioResponseDto>> UpdateMyProfile(PropietarioUpdateDto propietarioDto)
    {
      var idPropietario = int.Parse(User.FindFirst("idPropietario")?.Value ?? "0");

      // Validación de formato
      if (!string.IsNullOrEmpty(propietarioDto.Mail) && !IsValidEmail(propietarioDto.Mail))
      {
        return BadRequest("El formato del email es inválido");
      }

      // Verificar que el email no exista ya en la db (si se está intentando actualizarlo)
      if (!string.IsNullOrEmpty(propietarioDto.Mail))
      {
        var existeOtroPropietarioEmail = await _context.Propietarios
          .AnyAsync(p => p.Mail == propietarioDto.Mail && p.IdPropietario != idPropietario);

        if (existeOtroPropietarioEmail)
        {
          return Conflict("Los datos proporcionados ya están en uso");
        }
      }

      // Buscar el propietario actual
      var propietario = await _context.Propietarios
        .FirstOrDefaultAsync(p => p.IdPropietario == idPropietario);

      if (propietario == null)
      {
        return NotFound("Propietario no encontrado");
      }


      // Actualizar datos (solo campos permitidos, no DNI)
      if (!string.IsNullOrEmpty(propietarioDto.Apellido))
      {
        propietario.Apellido = propietarioDto.Apellido;
      }

      if (!string.IsNullOrEmpty(propietarioDto.Nombre))
      {
        propietario.Nombre = propietarioDto.Nombre;
      }

      if (!string.IsNullOrEmpty(propietarioDto.Mail))
      {
        propietario.Mail = propietarioDto.Mail;
      }

      propietario.Telefono = propietarioDto.Telefono;

      // Solo actualizar password si se proporciona
      if (!string.IsNullOrEmpty(propietarioDto.Password))
      {
        // Verificar el password antiguo
        var result = _passwordHasher.VerifyHashedPassword(propietario, propietario.Password, propietarioDto.OldPassword ?? "");

        if (result == PasswordVerificationResult.Failed)
        {
          return BadRequest("La contraseña antigua es incorrecta");
        }

        propietario.Password = _passwordHasher.HashPassword(propietario, propietarioDto.Password);
      }

      await _context.SaveChangesAsync();

      var response = new PropietarioResponseDto
      {
        IdPropietario = propietario.IdPropietario,
        Dni = propietario.Dni,
        Apellido = propietario.Apellido,
        Nombre = propietario.Nombre,
        Mail = propietario.Mail,
        Telefono = propietario.Telefono
      };

      return Ok(response);
    }

    private bool IsValidEmail(string email)
    {
      try
      {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
      }
      catch
      {
        return false;
      }
    }
  }
}
