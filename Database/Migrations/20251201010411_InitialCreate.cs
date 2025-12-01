using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reading_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    book_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    author_id = table.Column<int>(type: "int", nullable: false),
                    reading_status_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_book", x => x.id);
                    table.ForeignKey(
                        name: "fk_book_author_author_id",
                        column: x => x.author_id,
                        principalTable: "author",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_book_reading_status_reading_status_id",
                        column: x => x.reading_status_id,
                        principalTable: "reading_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_book_author_id",
                table: "book",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_book_reading_status_id",
                table: "book",
                column: "reading_status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "reading_status");
        }
    }
}
