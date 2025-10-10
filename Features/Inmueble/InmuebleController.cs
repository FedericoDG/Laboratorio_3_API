using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaApi.Features.Inmueble
{
  [Route("api/inmuebles")]
  [ApiController]
  public class InmuebleController : ControllerBase
  {
    private readonly InmobiliariaDbContext _context;

    public InmuebleController(InmobiliariaDbContext context)
    {
      _context = context;
    }

    // GET: api/inmuebles/me
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<InmuebleResponseDto>>> GetMyInmuebles()
    {
      var idPropietario = int.Parse(User.FindFirst("idPropietario")?.Value ?? "0");

      var inmuebles = await _context.Inmuebles
        .Include(i => i.Propietario)
        .Where(i => i.IdPropietario == idPropietario)
        .ToListAsync();

      var response = inmuebles.Select(i => new InmuebleResponseDto
      {
        IdInmueble = i.IdInmueble,
        Direccion = i.Direccion,
        Ambientes = i.Ambientes,
        Tipo = i.Tipo,
        Uso = i.Uso,
        Precio = i.Precio,
        Imagen = i.Imagen,
        Disponible = i.Disponible,
        IdPropietario = i.IdPropietario,
        PropietarioNombre = i.Propietario?.Nombre,
        PropietarioApellido = i.Propietario?.Apellido,
        PropietarioEmail = i.Propietario?.Mail,
        Inquilino = null, // no necesita datos del inquilino
        Alquiler = null // no necesita datos del alquiler
      });

      return Ok(response);
    }

    // GET: api/inmuebles/id/{id}
    [HttpGet("id/{id}")]
    public async Task<ActionResult<InmuebleResponseDto>> GetInmuebleById(int id)
    {
      var inmueble = await _context.Inmuebles
        .Include(i => i.Propietario)
        .FirstOrDefaultAsync(i => i.IdInmueble == id);

      if (inmueble == null) return NotFound();

      var response = new InmuebleResponseDto
      {
        IdInmueble = inmueble.IdInmueble,
        Direccion = inmueble.Direccion,
        Ambientes = inmueble.Ambientes,
        Tipo = inmueble.Tipo,
        Uso = inmueble.Uso,
        Precio = inmueble.Precio,
        Imagen = inmueble.Imagen,
        Disponible = inmueble.Disponible,
        IdPropietario = inmueble.IdPropietario,
        PropietarioNombre = inmueble.Propietario?.Nombre,
        PropietarioApellido = inmueble.Propietario?.Apellido,
        PropietarioEmail = inmueble.Propietario?.Mail,
        Inquilino = null, // no necesita datos del inquilino
        Alquiler = null // no necesita datos del alquiler
      };

      return Ok(response);
    }

    // POST: api/inmuebles
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<InmuebleResponseDto>> CreateInmueble(InmuebleCreateDto inmuebleDto)
    {
      var idPropietario = int.Parse(User.FindFirst("idPropietario")?.Value ?? "0");

      var inmueble = new Inmueble
      {
        Direccion = inmuebleDto.Direccion,
        Ambientes = inmuebleDto.Ambientes,
        Tipo = inmuebleDto.Tipo,
        Uso = inmuebleDto.Uso,
        Precio = inmuebleDto.Precio,
        Imagen = inmuebleDto.Imagen,
        Disponible = false,
        IdPropietario = idPropietario
      };

      _context.Inmuebles.Add(inmueble);
      await _context.SaveChangesAsync();

      await _context.Entry(inmueble)
        .Reference(i => i.Propietario)
        .LoadAsync();

      var response = new InmuebleResponseDto
      {
        IdInmueble = inmueble.IdInmueble,
        Direccion = inmueble.Direccion,
        Ambientes = inmueble.Ambientes,
        Tipo = inmueble.Tipo,
        Uso = inmueble.Uso,
        Precio = inmueble.Precio,
        Imagen = inmueble.Imagen,
        Disponible = inmueble.Disponible,
        IdPropietario = inmueble.IdPropietario,
        PropietarioNombre = inmueble.Propietario?.Nombre,
        PropietarioApellido = inmueble.Propietario?.Apellido,
        PropietarioEmail = inmueble.Propietario?.Mail,
        Inquilino = null, // no necesita datos del inquilino
        Alquiler = null // no necesita datos del alquiler
      };

      return Ok(response);
    }

    // PUT: api/inmuebles/id/{id}
    [HttpPut("id/{id}")]
    [Authorize]
    public async Task<ActionResult<InmuebleResponseDto>> UpdateDisponible(int id, InmuebleDisponibleDto dto)
    {
      var idPropietario = int.Parse(User.FindFirst("idPropietario")?.Value ?? "0");

      var inmueble = await _context.Inmuebles
        .Include(i => i.Propietario)
        .FirstOrDefaultAsync(i => i.IdInmueble == id);

      if (inmueble == null) return NotFound();
      if (inmueble.IdPropietario != idPropietario) return Forbid();

      inmueble.Disponible = dto.Disponible;
      await _context.SaveChangesAsync();

      var response = new InmuebleResponseDto
      {
        IdInmueble = inmueble.IdInmueble,
        Direccion = inmueble.Direccion,
        Ambientes = inmueble.Ambientes,
        Tipo = inmueble.Tipo,
        Uso = inmueble.Uso,
        Precio = inmueble.Precio,
        Imagen = inmueble.Imagen,
        Disponible = inmueble.Disponible,
        IdPropietario = inmueble.IdPropietario,
        PropietarioNombre = inmueble.Propietario?.Nombre,
        PropietarioApellido = inmueble.Propietario?.Apellido,
        PropietarioEmail = inmueble.Propietario?.Mail,
        Inquilino = null, // no necesita datos del inquilino
        Alquiler = null // no necesita datos del alquiler
      };

      return Ok(response);
    }

    // GET: api/inmuebles/mis-contratos-activos
    [HttpGet("mis-contratos-activos")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<InmuebleResponseDto>>> GetMisInmueblesConContratosActivos()
    {
      var idPropietario = int.Parse(User.FindFirst("idPropietario")?.Value ?? "0");
      var fechaActual = DateTime.Now;

      // Esto fue un caos
      // Cargar los inmuebles con sus propietarios y alquileres activos
      var inmuebles = await _context.Inmuebles
        .Include(i => i.Propietario)
        .Include(i => i.Alquileres.Where(a => a.FechaInicio <= fechaActual && a.FechaFin >= fechaActual))
        .ThenInclude(a => a.Inquilino)
        .Include(i => i.Alquileres.Where(a => a.FechaInicio <= fechaActual && a.FechaFin >= fechaActual))
        .ThenInclude(a => a.Pagos)
        .Where(i => i.IdPropietario == idPropietario &&
                    i.Alquileres.Any(a => a.FechaInicio <= fechaActual && a.FechaFin >= fechaActual))
        .ToListAsync();

      var response = inmuebles.Select(i =>
      {
        // Alquiler vigente (uno por inmueble)
        var alquilerVigente = i.Alquileres
          .FirstOrDefault(a => a.FechaInicio <= fechaActual && a.FechaFin >= fechaActual);

        return new InmuebleResponseDto
        {
          IdInmueble = i.IdInmueble,
          Direccion = i.Direccion,
          Ambientes = i.Ambientes,
          Tipo = i.Tipo,
          Uso = i.Uso,
          Precio = i.Precio,
          Imagen = i.Imagen,
          Disponible = i.Disponible,
          IdPropietario = i.IdPropietario,
          PropietarioNombre = i.Propietario?.Nombre,
          PropietarioApellido = i.Propietario?.Apellido,
          PropietarioEmail = i.Propietario?.Mail,

          // Inquilino del contrato vigente
          Inquilino = alquilerVigente?.Inquilino != null ? new InquilinoEnRespuestaDto
          {
            IdInquilino = alquilerVigente.Inquilino.IdInquilino,
            Dni = alquilerVigente.Inquilino.Dni,
            Apellido = alquilerVigente.Inquilino.Apellido,
            Nombre = alquilerVigente.Inquilino.Nombre,
            Direccion = alquilerVigente.Inquilino.Direccion,
            Telefono = alquilerVigente.Inquilino.Telefono
          } : null,

          // Loss pagos
          Alquiler = alquilerVigente != null ? new ContratoCompletoDto
          {
            IdAlquiler = alquilerVigente.IdAlquiler,
            Precio = alquilerVigente.Precio,
            FechaInicio = alquilerVigente.FechaInicio,
            FechaFin = alquilerVigente.FechaFin,
            Pagos = alquilerVigente.Pagos.Select(p => new PagoEnContratoDto
            {
              IdPago = p.IdPago,
              NroPago = p.NroPago,
              Fecha = p.Fecha,
              Importe = p.Importe
            }).OrderBy(p => p.NroPago).ToList()
          } : null
        };
      });

      return Ok(response);
    }
  }
}