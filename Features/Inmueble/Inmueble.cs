using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropietarioModel = InmobiliariaApi.Features.Propietario.Propietario;

namespace InmobiliariaApi.Features.Inmueble
{
  [Table("inmuebles")]
  public class Inmueble
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_inmueble")]
    public int IdInmueble { get; set; }

    [Column("direccion")]
    public string Direccion { get; set; } = string.Empty;

    [Column("ambientes")]
    public int Ambientes { get; set; }

    [Column("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [Column("uso")]
    public string Uso { get; set; } = string.Empty;

    [Column("precio")]
    public double Precio { get; set; }

    [Column("imagen")]
    public string? Imagen { get; set; }

    [Column("disponible")]
    public bool Disponible { get; set; } = true;

    // Fks
    [Column("id_propietario")]
    public int IdPropietario { get; set; }

    // Navegación hacia Propietario
    [ForeignKey("IdPropietario")]
    public virtual PropietarioModel Propietario { get; set; } = null!;

    // Navegación hacia Alquileres
    public virtual ICollection<Features.Alquiler.Alquiler> Alquileres { get; set; } = [];
  }
}