using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autorizacion.Migrations
{
    /// <inheritdoc />
    public partial class AyudaComunasYRegiones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AyudaComuna",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CiudadId = table.Column<int>(type: "int", nullable: false),
                    AyudaSocialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AyudaComuna", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AyudaComuna_AyudasSociales_AyudaSocialId",
                        column: x => x.AyudaSocialId,
                        principalTable: "AyudasSociales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AyudaComuna_Ciudad_CiudadId",
                        column: x => x.CiudadId,
                        principalTable: "Ciudades",
                        principalColumn: "CiudadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AyudaRegion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    AyudaSocialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AyudaRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AyudaRegion_AyudasSociales_AyudaSocialId",
                        column: x => x.AyudaSocialId,
                        principalTable: "AyudasSociales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AyudaRegion_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regiones",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AyudaComuna_AyudaSocialId",
                table: "AyudaComuna",
                column: "AyudaSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_AyudaComuna_CiudadId",
                table: "AyudaComuna",
                column: "CiudadId");

            migrationBuilder.CreateIndex(
                name: "IX_AyudaRegion_AyudaSocialId",
                table: "AyudaRegion",
                column: "AyudaSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_AyudaRegion_RegionId",
                table: "AyudaRegion",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AyudaComuna");

            migrationBuilder.DropTable(
                name: "AyudaRegion");
        }
    }
}
