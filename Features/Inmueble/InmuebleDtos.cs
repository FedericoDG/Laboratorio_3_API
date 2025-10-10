namespace InmobiliariaApi.Features.Inmueble
{
  public class InmuebleCreateDto
  {
    // DTO para crear un inmueble
    public string Direccion { get; set; } = string.Empty;
    public int Ambientes { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Uso { get; set; } = string.Empty;
    public double Precio { get; set; }
    public string? Imagen { get; set; }
    public bool Disponible { get; set; } = true;
  }

  // DTO para actualizar la disponibilidad de un inmueble
  public class InmuebleDisponibleDto
  {
    public bool Disponible { get; set; }
  }

  //  DTO para el inquilino en la respuesta
  public class InquilinoEnRespuestaDto
  {
    public int IdInquilino { get; set; }
    public int Dni { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public int Telefono { get; set; }
  }

  // DTO para pago en la respuesta del contrato
  public class PagoEnContratoDto
  {
    public int IdPago { get; set; }
    public int NroPago { get; set; }
    public DateTime Fecha { get; set; }
    public double Importe { get; set; }
  }

  // DTO para contrato completo con pagos
  public class ContratoCompletoDto
  {
    public int IdAlquiler { get; set; }
    public double Precio { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public List<PagoEnContratoDto> Pagos { get; set; } = new List<PagoEnContratoDto>();
  }

  // DTO para la respuesta del inmueble, incluye datos del propietario (opcionalmente inquilino y alquiler)
  public class InmuebleResponseDto
  {
    public int IdInmueble { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public int Ambientes { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Uso { get; set; } = string.Empty;
    public double Precio { get; set; }
    public string? Imagen { get; set; }
    public bool Disponible { get; set; }
    public int IdPropietario { get; set; }
    public string? PropietarioNombre { get; set; }
    public string? PropietarioApellido { get; set; }
    public string? PropietarioEmail { get; set; }

    // DTO Inquilino  (solo para inmuebles con contratos vigentes)
    public InquilinoEnRespuestaDto? Inquilino { get; set; }

    // DTO Alquiler completo con pagos (ndpoints específicos)
    public ContratoCompletoDto? Alquiler { get; set; }
  }
}