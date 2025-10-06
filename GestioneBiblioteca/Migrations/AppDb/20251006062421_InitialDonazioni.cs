using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneBiblioteca.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitialDonazioni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Importo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataDonazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PagamentoRiuscito = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donazioni", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donazioni");
        }
    }
}
