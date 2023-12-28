using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteKeyworks.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avaliacao",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "ComentarioAvaliacao",
                table: "Filme");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Filme",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Filme_Titulo",
                table: "Filme",
                column: "Titulo",
                unique: true,
                filter: "[Titulo] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filme_Titulo",
                table: "Filme");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Avaliacao",
                table: "Filme",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComentarioAvaliacao",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
