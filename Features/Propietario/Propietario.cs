using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Features.Propietario
{
  [Table("propietarios")]
  public class Propietario
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_propietario")]
    public int IdPropietario { get; set; }

    [Column("dni")]
    public int Dni { get; set; }

    [Column("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("mail")]
    public string Mail { get; set; } = string.Empty;

    [Column("telefono")]
    public int Telefono { get; set; }

    [Column("password")]
    public string Password { get; set; } = string.Empty;
  }
}