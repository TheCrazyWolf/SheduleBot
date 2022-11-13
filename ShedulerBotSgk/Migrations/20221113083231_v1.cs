using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShedulerBotSgk.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdGroup = table.Column<long>(type: "INTEGER", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    Timer = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminID = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    IdTask = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeTask = table.Column<char>(type: "TEXT", nullable: true),
                    PeerId = table.Column<long>(type: "INTEGER", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    ResultText = table.Column<string>(type: "TEXT", nullable: true),
                    Settingid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.IdTask);
                    table.ForeignKey(
                        name: "FK_Tasks_Settings_Settingid",
                        column: x => x.Settingid,
                        principalTable: "Settings",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Settingid",
                table: "Tasks",
                column: "Settingid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
