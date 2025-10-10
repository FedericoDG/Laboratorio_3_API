using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InmobiliariaApi.Features.Propietario
{
  [Route("api/propietarios")]
  [ApiController]
  public class PropietarioController : ControllerBase
  {
    private readonly InmobiliariaDbContext _context;

    public PropietarioController(InmobiliariaDbContext context)
    {
      _context = context;
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

      // Verificar si existe otro propietario con el mismo DNI)
      var existeOtroPropietario = await _context.Propietarios
        .AnyAsync(p => p.Dni == propietarioDto.Dni && p.IdPropietario != idPropietario);

      if (existeOtroPropietario)
      {
        return Conflict($"Ya existe otro propietario con el DNI {propietarioDto.Dni}");
      }

      // Buscar el propietario actual
      var propietario = await _context.Propietarios
        .FirstOrDefaultAsync(p => p.IdPropietario == idPropietario);

      if (propietario == null)
      {
        return NotFound("Propietario no encontrado");
      }

      // Actualizar datos
      propietario.Dni = propietarioDto.Dni;
      propietario.Apellido = propietarioDto.Apellido;
      propietario.Nombre = propietarioDto.Nombre;
      propietario.Mail = propietarioDto.Mail;
      propietario.Telefono = propietarioDto.Telefono;

      // Solo actualizar password sólo si se proporciona. Caso contrario "siga siga"
      if (!string.IsNullOrEmpty(propietarioDto.Password))
      {
        propietario.Password = propietarioDto.Password;
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
  }
}
