using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Features.Inquilino
{
  [Table("inquilinos")]
  public class Inquilino
  {
    [Key]
    [Column("id_inquilino")]
    public int IdInquilino { get; set; }

    [Column("dni")]
    public int Dni { get; set; }

    [Column("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("direccion")]
    public string Direccion { get; set; } = string.Empty;

    [Column("telefono")]
    public int Telefono { get; set; }

    // Navegación hacia Alquileres
    public virtual ICollection<Features.Alquiler.Alquiler> Alquileres { get; set; } = [];
  }
}