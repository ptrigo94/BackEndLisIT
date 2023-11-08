using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autorizacion.Migrations
{
    /// <inheritdoc />
    public partial class Full : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CiudadId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            


            migrationBuilder.CreateIndex(
                name: "IX_Users_CiudadId",
                table: "Users",
                column: "CiudadId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            

            migrationBuilder.DropIndex(
                name: "IX_Users_CiudadId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CiudadId",
                table: "Users");
        }
    }
}
