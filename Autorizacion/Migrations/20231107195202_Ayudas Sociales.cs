using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autorizacion.Migrations
{
    /// <inheritdoc />
    public partial class AyudasSociales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Anio",
                table: "AyudasSociales",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Anio",
                table: "AyudasSociales",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
