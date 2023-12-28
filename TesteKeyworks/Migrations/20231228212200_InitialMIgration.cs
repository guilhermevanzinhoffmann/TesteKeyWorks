using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteKeyworks.Migrations
{
    /// <inheritdoc />
    public partial class InitialMIgration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filme",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streaming",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streaming", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilmeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoess_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmesStreamings",
                columns: table => new
                {
                    FilmeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreamingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesStreamings", x => new { x.StreamingId, x.FilmeId });
                    table.ForeignKey(
                        name: "FK_FilmesStreamings_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmesStreamings_Streaming_StreamingId",
                        column: x => x.StreamingId,
                        principalTable: "Streaming",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoess_FilmeId",
                table: "Avaliacoess",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_Filme_Titulo",
                table: "Filme",
                column: "Titulo",
                unique: true,
                filter: "[Titulo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FilmesStreamings_FilmeId",
                table: "FilmesStreamings",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_Streaming_Nome",
                table: "Streaming",
                column: "Nome",
                unique: true,
                filter: "[Nome] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoess");

            migrationBuilder.DropTable(
                name: "FilmesStreamings");

            migrationBuilder.DropTable(
                name: "Filme");

            migrationBuilder.DropTable(
                name: "Streaming");
        }
    }
}
