using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Control_de_Lotes.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDestinoASiembra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destino",
                table: "Siembra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destino",
                table: "Siembra");
        }
    }
}
