namespace InmobiliariaApi.Features.Propietario
{
  public class PropietarioUpdateDto
  {
    // DTO para actualizar propietario
    public int Dni { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int Telefono { get; set; }
    public string? Password { get; set; }
    public string? OldPassword { get; set; }
  }

  // DTO para la respuesta de propietario (sin password, obviamente...)
  public class PropietarioResponseDto
  {
    public int IdPropietario { get; set; }
    public int Dni { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int Telefono { get; set; }
  }
}