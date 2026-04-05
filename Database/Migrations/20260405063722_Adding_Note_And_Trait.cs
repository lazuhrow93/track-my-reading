using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Note_And_Trait : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    character_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    created_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note", x => x.id);
                    table.ForeignKey(
                        name: "fk_note_character_character_id",
                        column: x => x.character_id,
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trait",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    created_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trait", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "character_trait",
                columns: table => new
                {
                    characters_id = table.Column<int>(type: "int", nullable: false),
                    traits_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_trait", x => new { x.characters_id, x.traits_id });
                    table.ForeignKey(
                        name: "fk_character_trait_character_characters_id",
                        column: x => x.characters_id,
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_character_trait_trait_traits_id",
                        column: x => x.traits_id,
                        principalTable: "trait",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_trait_traits_id",
                table: "character_trait",
                column: "traits_id");

            migrationBuilder.CreateIndex(
                name: "ix_note_character_id",
                table: "note",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_trait");

            migrationBuilder.DropTable(
                name: "note");

            migrationBuilder.DropTable(
                name: "trait");
        }
    }
}
