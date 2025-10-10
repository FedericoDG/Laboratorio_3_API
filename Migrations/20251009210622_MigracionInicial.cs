using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InmobiliariaApi.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inquilinos",
                columns: table => new
                {
                    id_inquilino = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dni = table.Column<int>(type: "int", nullable: false),
                    apellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inquilinos", x => x.id_inquilino);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "propietarios",
                columns: table => new
                {
                    id_propietario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dni = table.Column<int>(type: "int", nullable: false),
                    apellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_propietarios", x => x.id_propietario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inmuebles",
                columns: table => new
                {
                    id_inmueble = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    direccion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ambientes = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    uso = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<double>(type: "double", nullable: false),
                    imagen = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    disponible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    id_propietario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inmuebles", x => x.id_inmueble);
                    table.ForeignKey(
                        name: "FK_inmuebles_propietarios_id_propietario",
                        column: x => x.id_propietario,
                        principalTable: "propietarios",
                        principalColumn: "id_propietario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alquileres",
                columns: table => new
                {
                    id_alquiler = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    precio = table.Column<double>(type: "double", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    id_inquilino = table.Column<int>(type: "int", nullable: false),
                    id_inmueble = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alquileres", x => x.id_alquiler);
                    table.ForeignKey(
                        name: "FK_alquileres_inmuebles_id_inmueble",
                        column: x => x.id_inmueble,
                        principalTable: "inmuebles",
                        principalColumn: "id_inmueble",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_alquileres_inquilinos_id_inquilino",
                        column: x => x.id_inquilino,
                        principalTable: "inquilinos",
                        principalColumn: "id_inquilino",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    id_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nro_pago = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    importe = table.Column<double>(type: "double", nullable: false),
                    id_alquiler = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos", x => x.id_pago);
                    table.ForeignKey(
                        name: "FK_pagos_alquileres_id_alquiler",
                        column: x => x.id_alquiler,
                        principalTable: "alquileres",
                        principalColumn: "id_alquiler",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "inquilinos",
                columns: new[] { "id_inquilino", "apellido", "direccion", "dni", "nombre", "telefono" },
                values: new object[,]
                {
                    { 1, "Pérez", "Belgrano 456, CABA", 11111111, "Ana María", 1598765432 },
                    { 2, "García", "Santa Fe 789, Palermo", 22222222, "Luis Fernando", 1587654321 },
                    { 3, "Silva", "Rivadavia 1234, Caballito", 33333333, "Carmen Elena", 1576543210 }
                });

            migrationBuilder.InsertData(
                table: "propietarios",
                columns: new[] { "id_propietario", "apellido", "dni", "mail", "nombre", "password", "telefono" },
                values: new object[,]
                {
                    { 1, "González", 12345678, "juan.gonzalez@email.com", "Juan Carlos", "AQAAAAIAAYagAAAAEAI7vDzjljLddAK4n9tI3jkvoi2qTG6x8M6xnZ4JMDi/tn/WLtyd/EVn2qJ6M8pzVA==", 1234567890 },
                    { 2, "Martínez", 23456789, "maria.martinez@email.com", "María Elena", "AQAAAAIAAYagAAAAEAI7vDzjljLddAK4n9tI3jkvoi2qTG6x8M6xnZ4JMDi/tn/WLtyd/EVn2qJ6M8pzVA==", 1234567890 },
                    { 3, "López", 34567890, "carlos.lopez@email.com", "Carlos Alberto", "AQAAAAIAAYagAAAAEAI7vDzjljLddAK4n9tI3jkvoi2qTG6x8M6xnZ4JMDi/tn/WLtyd/EVn2qJ6M8pzVA==", 1234567890 }
                });

            migrationBuilder.InsertData(
                table: "inmuebles",
                columns: new[] { "id_inmueble", "ambientes", "direccion", "disponible", "id_propietario", "imagen", "precio", "tipo", "uso" },
                values: new object[,]
                {
                    { 1, 3, "Av. Corrientes 1234, CABA", true, 1, "https://example.com/depto1.jpg", 150000.0, "Departamento", "Residencial" },
                    { 2, 4, "San Martín 567, Belgrano", true, 1, "https://example.com/casa1.jpg", 280000.0, "Casa", "Residencial" },
                    { 3, 2, "Florida 890, Microcentro", false, 2, "https://example.com/local1.jpg", 95000.0, "Local", "Comercial" },
                    { 4, 5, "Libertador 2345, Palermo", true, 3, "https://example.com/casa2.jpg", 450000.0, "Casa", "Residencial" },
                    { 5, 2, "Rivadavia 3456, Caballito", true, 2, "https://example.com/depto2.jpg", 120000.0, "Departamento", "Residencial" }
                });

            migrationBuilder.InsertData(
                table: "alquileres",
                columns: new[] { "id_alquiler", "fecha_fin", "fecha_inicio", "id_inmueble", "id_inquilino", "precio" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 75000.0 },
                    { 2, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 120000.0 }
                });

            migrationBuilder.InsertData(
                table: "pagos",
                columns: new[] { "id_pago", "fecha", "id_alquiler", "importe", "nro_pago" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 1 },
                    { 2, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 2 },
                    { 3, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 3 },
                    { 4, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 4 },
                    { 5, new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 5 },
                    { 6, new DateTime(2024, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 120000.0, 1 },
                    { 7, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 120000.0, 2 },
                    { 8, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 120000.0, 3 },
                    { 9, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 120000.0, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alquiler_FechaFin",
                table: "alquileres",
                column: "fecha_fin");

            migrationBuilder.CreateIndex(
                name: "IX_Alquiler_FechaInicio",
                table: "alquileres",
                column: "fecha_inicio");

            migrationBuilder.CreateIndex(
                name: "IX_Alquiler_Fechas",
                table: "alquileres",
                columns: new[] { "fecha_inicio", "fecha_fin" });

            migrationBuilder.CreateIndex(
                name: "IX_Alquiler_Inmueble",
                table: "alquileres",
                column: "id_inmueble");

            migrationBuilder.CreateIndex(
                name: "IX_Alquiler_Inquilino",
                table: "alquileres",
                column: "id_inquilino");

            migrationBuilder.CreateIndex(
                name: "IX_Inmueble_Disponible",
                table: "inmuebles",
                column: "disponible");

            migrationBuilder.CreateIndex(
                name: "IX_Inmueble_Propietario",
                table: "inmuebles",
                column: "id_propietario");

            migrationBuilder.CreateIndex(
                name: "IX_Inmueble_Tipo",
                table: "inmuebles",
                column: "tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Inmueble_Uso",
                table: "inmuebles",
                column: "uso");

            migrationBuilder.CreateIndex(
                name: "IX_Inquilino_Dni",
                table: "inquilinos",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Alquiler",
                table: "pagos",
                column: "id_alquiler");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Fecha",
                table: "pagos",
                column: "fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_NroPago",
                table: "pagos",
                column: "nro_pago");

            migrationBuilder.CreateIndex(
                name: "IX_Propietario_Dni",
                table: "propietarios",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propietario_Mail",
                table: "propietarios",
                column: "mail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "alquileres");

            migrationBuilder.DropTable(
                name: "inmuebles");

            migrationBuilder.DropTable(
                name: "inquilinos");

            migrationBuilder.DropTable(
                name: "propietarios");
        }
    }
}
