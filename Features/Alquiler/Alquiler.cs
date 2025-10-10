using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Features.Alquiler
{
  [Table("alquileres")]
  public class Alquiler
  {
    [Key]
    [Column("id_alquiler")]
    public int IdAlquiler { get; set; }

    [Column("precio")]
    public double Precio { get; set; }

    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime FechaFin { get; set; }

    // FKs
    [Column("id_inquilino")]
    public int IdInquilino { get; set; }

    [Column("id_inmueble")]
    public int IdInmueble { get; set; }

    // Navegación hacia Inquilino
    public Inquilino.Inquilino Inquilino { get; set; } = null!;

    // Navegación hacia Inmueble
    public Inmueble.Inmueble Inmueble { get; set; } = null!;

    // Navegación hacia Pagos
    public virtual ICollection<Features.Pago.Pago> Pagos { get; set; } = [];
  }
}