using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agromercado.AppMVC.Migrations
{
    public partial class AjusteClienteQuitarNitNrc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔥 ELIMINAR NIT
            migrationBuilder.DropColumn(
                name: "Nit",
                table: "Clientes");

            // 🔥 ELIMINAR NRC
            migrationBuilder.DropColumn(
                name: "Nrc",
                table: "Clientes");

            // 🔥 CAMBIAR ACTIVO A NOT NULL
            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 🔁 VOLVER ACTIVO NULLABLE
            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Clientes",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            // 🔁 VOLVER A CREAR NIT
            migrationBuilder.AddColumn<string>(
                name: "Nit",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            // 🔁 VOLVER A CREAR NRC
            migrationBuilder.AddColumn<string>(
                name: "Nrc",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}