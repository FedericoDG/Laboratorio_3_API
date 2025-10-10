using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Features.Pago
{
  [Table("pagos")]
  public class Pago
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_pago")]
    public int IdPago { get; set; }

    [Column("nro_pago")]
    public int NroPago { get; set; }

    [Column("fecha")]
    public DateTime Fecha { get; set; }

    [Column("importe")]
    public double Importe { get; set; }

    // FK
    [Column("id_alquiler")]
    public int IdAlquiler { get; set; }

    // Navegación hacia Alquiler
    public virtual Alquiler.Alquiler Alquiler { get; set; } = null!;  // Sin el ! no compila (misterio...)
  }
}