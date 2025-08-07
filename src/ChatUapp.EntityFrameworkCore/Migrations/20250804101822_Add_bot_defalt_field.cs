using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatUapp.Migrations
{
    /// <inheritdoc />
    public partial class Add_bot_defalt_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDefalt",
                schema: "bot",
                table: "ChatBots",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDefalt",
                schema: "bot",
                table: "ChatBots");
        }
    }
}
