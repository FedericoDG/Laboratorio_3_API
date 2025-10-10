using Microsoft.EntityFrameworkCore;
using InmobiliariaApi.Features.Propietario;
using InmobiliariaApi.Features.Inmueble;
using InmobiliariaApi.Features.Inquilino;
using InmobiliariaApi.Features.Alquiler;
using InmobiliariaApi.Features.Pago;

namespace InmobiliariaApi
{
  public class InmobiliariaDbContext(DbContextOptions<InmobiliariaDbContext> options) : DbContext(options)
  {
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; }
    public DbSet<Inquilino> Inquilinos { get; set; }
    public DbSet<Alquiler> Alquileres { get; set; }
    public DbSet<Pago> Pagos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Propietario
      modelBuilder.Entity<Propietario>(entity =>
      {
        // Índice para login por email
        entity.HasIndex(e => e.Mail)
              .IsUnique()
              .HasDatabaseName("IX_Propietario_Mail");
      });

      // Inmueble
      modelBuilder.Entity<Inmueble>(entity =>
      {
        // Relación Inmueble -> Propietario (Muchos a Uno)
        entity.HasOne(i => i.Propietario)
              .WithMany() // Un propietario puede tener muchos inmuebles
              .HasForeignKey(i => i.IdPropietario)
              .OnDelete(DeleteBehavior.Restrict);

        // Índice para filtrar inmuebles por propietario (JWT)
        entity.HasIndex(e => e.IdPropietario)
              .HasDatabaseName("IX_Inmueble_Propietario");
      });

      // Inquilino
      modelBuilder.Entity<Inquilino>(entity =>
      {
      });

      // Alquiler
      modelBuilder.Entity<Alquiler>(entity =>
      {
        // Relación Alquiler -> Inquilino (Muchos a Uno)
        entity.HasOne(a => a.Inquilino)
              .WithMany(i => i.Alquileres) // Un inquilino puede tener muchos alquileres
              .HasForeignKey(a => a.IdInquilino)
              .OnDelete(DeleteBehavior.Restrict);

        // Relación Alquiler -> Inmueble (Muchos a Uno)
        entity.HasOne(a => a.Inmueble)
              .WithMany(i => i.Alquileres) // Un inmueble puede tener muchos alquileres
              .HasForeignKey(a => a.IdInmueble)
              .OnDelete(DeleteBehavior.Restrict);

        // Índice para mostrar contratos de un inmueble
        entity.HasIndex(e => e.IdInmueble)
              .HasDatabaseName("IX_Alquiler_Inmueble");
      });

      // Pago
      modelBuilder.Entity<Pago>(entity =>
      {
        // Relación Pago -> Alquiler (Muchos a Uno)
        entity.HasOne(p => p.Alquiler)
              .WithMany(a => a.Pagos) // Un alquiler puede tener muchos pagos
              .HasForeignKey(p => p.IdAlquiler)
              .OnDelete(DeleteBehavior.Restrict);

        // Índice para obtener pagos de un contrato
        entity.HasIndex(e => e.IdAlquiler)
              .HasDatabaseName("IX_Pago_Alquiler");
      });

      // Seed Data
      SeedPropietarios(modelBuilder);
      SeedInmuebles(modelBuilder);
      SeedInquilinos(modelBuilder);
      SeedAlquileres(modelBuilder);
      SeedPagos(modelBuilder);
    }

    private static void SeedPropietarios(ModelBuilder modelBuilder)
    {
      // Contraseña hasheada estática para "1234"
      var hashedPassword = "AQAAAAIAAYagAAAAEAI7vDzjljLddAK4n9tI3jkvoi2qTG6x8M6xnZ4JMDi/tn/WLtyd/EVn2qJ6M8pzVA==";

      var propietarios = new List<Propietario>
      {
        new() {
          IdPropietario = 1,
          Dni = 12345678,
          Apellido = "González",
          Nombre = "Juan Carlos",
          Mail = "juan.gonzalez@email.com",
          Telefono = 1234567890,
          Password = hashedPassword
        },
        new() {
          IdPropietario = 2,
          Dni = 23456789,
          Apellido = "Martínez",
          Nombre = "María Elena",
          Mail = "maria.martinez@email.com",
          Telefono = 1234567890,
          Password = hashedPassword
        },
        new() {
          IdPropietario = 3,
          Dni = 34567890,
          Apellido = "López",
          Nombre = "Carlos Alberto",
          Mail = "carlos.lopez@email.com",
          Telefono = 1234567890,
          Password = hashedPassword
        }
      };

      modelBuilder.Entity<Propietario>().HasData(propietarios);
    }

    private static void SeedInmuebles(ModelBuilder modelBuilder)
    {
      var inmuebles = new List<Inmueble>
      {
        new() {
          IdInmueble = 1,
          Direccion = "Av. Corrientes 1234, CABA",
          Ambientes = 3,
          Tipo = "Departamento",
          Uso = "Residencial",
          Precio = 150000.00,
          Imagen = "https://example.com/depto1.jpg",
          Disponible = true,
          IdPropietario = 1 // Juan Carlos González
        },
        new() {
          IdInmueble = 2,
          Direccion = "San Martín 567, Belgrano",
          Ambientes = 4,
          Tipo = "Casa",
          Uso = "Residencial",
          Precio = 280000.00,
          Imagen = "https://example.com/casa1.jpg",
          Disponible = true,
          IdPropietario = 1 // Juan Carlos González
        },
        new() {
          IdInmueble = 3,
          Direccion = "Florida 890, Microcentro",
          Ambientes = 2,
          Tipo = "Local",
          Uso = "Comercial",
          Precio = 95000.00,
          Imagen = "https://example.com/local1.jpg",
          Disponible = false,
          IdPropietario = 2 // María Elena Martínez
        },
        new() {
          IdInmueble = 4,
          Direccion = "Libertador 2345, Palermo",
          Ambientes = 5,
          Tipo = "Casa",
          Uso = "Residencial",
          Precio = 450000.00,
          Imagen = "https://example.com/casa2.jpg",
          Disponible = true,
          IdPropietario = 3 // Carlos Alberto López
        },
        new() {
          IdInmueble = 5,
          Direccion = "Rivadavia 3456, Caballito",
          Ambientes = 2,
          Tipo = "Departamento",
          Uso = "Residencial",
          Precio = 120000.00,
          Imagen = "https://example.com/depto2.jpg",
          Disponible = true,
          IdPropietario = 2 // María Elena Martínez
        }
      };

      modelBuilder.Entity<Inmueble>().HasData(inmuebles);
    }

    private static void SeedInquilinos(ModelBuilder modelBuilder)
    {
      var inquilinos = new List<Inquilino>
      {
        new() {
          IdInquilino = 1,
          Dni = 11111111,
          Apellido = "Pérez",
          Nombre = "Ana María",
          Direccion = "Belgrano 456, CABA",
          Telefono = 1598765432
        },
        new() {
          IdInquilino = 2,
          Dni = 22222222,
          Apellido = "García",
          Nombre = "Luis Fernando",
          Direccion = "Santa Fe 789, Palermo",
          Telefono = 1587654321
        },
        new() {
          IdInquilino = 3,
          Dni = 33333333,
          Apellido = "Silva",
          Nombre = "Carmen Elena",
          Direccion = "Rivadavia 1234, Caballito",
          Telefono = 1576543210
        }
      };

      modelBuilder.Entity<Inquilino>().HasData(inquilinos);
    }

    private static void SeedAlquileres(ModelBuilder modelBuilder)
    {
      var alquileres = new List<Alquiler>
      {
        new() {
          IdAlquiler = 1,
          Precio = 75000.00,
          FechaInicio = new DateTime(2024, 1, 1),
          FechaFin = new DateTime(2025, 12, 31),
          IdInquilino = 1, // Ana María Pérez
          IdInmueble = 1   // Av. Corrientes 1234, CABA
        },
        new() {
          IdAlquiler = 2,
          Precio = 120000.00,
          FechaInicio = new DateTime(2024, 11, 1),
          FechaFin = new DateTime(2025, 11, 30),
          IdInquilino = 2, // Luis Fernando García
          IdInmueble = 3   // Florida 890, Microcentro
        }
      };

      modelBuilder.Entity<Alquiler>().HasData(alquileres);
    }

    private static void SeedPagos(ModelBuilder modelBuilder)
    {
      var pagos = new List<Pago>
      {
        // IdAlquiler = 1
        new() {
          IdPago = 1,
          NroPago = 1,
          Fecha = new DateTime(2024, 1, 15),
          Importe = 75000.00,
          IdAlquiler = 1
        },
        new() {
          IdPago = 2,
          NroPago = 2,
          Fecha = new DateTime(2024, 2, 15),
          Importe = 75000.00,
          IdAlquiler = 1
        },
        new() {
          IdPago = 3,
          NroPago = 3,
          Fecha = new DateTime(2024, 3, 15),
          Importe = 75000.00,
          IdAlquiler = 1
        },
        new() {
          IdPago = 4,
          NroPago = 4,
          Fecha = new DateTime(2024, 4, 15),
          Importe = 75000.00,
          IdAlquiler = 1
        },
        new() {
          IdPago = 5,
          NroPago = 5,
          Fecha = new DateTime(2024, 5, 15),
          Importe = 75000.00,
          IdAlquiler = 1
        },
        
        // IdAlquiler = 2
        new() {
          IdPago = 6,
          NroPago = 1,
          Fecha = new DateTime(2024, 11, 15),
          Importe = 120000.00,
          IdAlquiler = 2
        },
        new() {
          IdPago = 7,
          NroPago = 2,
          Fecha = new DateTime(2024, 12, 15),
          Importe = 120000.00,
          IdAlquiler = 2
        },
        new() {
          IdPago = 8,
          NroPago = 3,
          Fecha = new DateTime(2025, 1, 15),
          Importe = 120000.00,
          IdAlquiler = 2
        },
        new() {
          IdPago = 9,
          NroPago = 4,
          Fecha = new DateTime(2025, 2, 15),
          Importe = 120000.00,
          IdAlquiler = 2
        }
      };

      modelBuilder.Entity<Pago>().HasData(pagos);
    }
  }
}
