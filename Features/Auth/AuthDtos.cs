using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApi.Features.Auth
{
  // DTO para la solicitud de login
  public class LoginRequestDto
  {
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
  }

  // DTO para la respuesta de login
  public class SimpleLoginResponseDto
  {
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
  }

  // DTO para la información del propietario en respuesta de /me
  public class PropietarioAuthDto
  {
    public int IdPropietario { get; set; }
    public int Dni { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int Telefono { get; set; } = 0;
  }
}