using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatUapp.Migrations
{
    /// <inheritdoc />
    public partial class fix_vulnurable_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDefalt",
                schema: "bot",
                table: "ChatBots",
                newName: "isDefault");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDefault",
                schema: "bot",
                table: "ChatBots",
                newName: "isDefalt");
        }
    }
}
