using Microsoft.EntityFrameworkCore.Migrations;

namespace TgBotApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserVocabs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVocabs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocabItems",
                columns: table => new
                {
                    VocabItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserVocabId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabItems", x => x.VocabItemId);
                    table.ForeignKey(
                        name: "FK_VocabItems_UserVocabs_UserVocabId",
                        column: x => x.UserVocabId,
                        principalTable: "UserVocabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocabItems_UserVocabId",
                table: "VocabItems",
                column: "UserVocabId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocabItems");

            migrationBuilder.DropTable(
                name: "UserVocabs");
        }
    }
}
