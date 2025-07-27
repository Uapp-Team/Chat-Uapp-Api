using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatUapp.Migrations
{
    /// <inheritdoc />
    public partial class Change_FlagMaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Flag",
                schema: "bot",
                table: "ChatSesions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Flag",
                schema: "bot",
                table: "ChatSesions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
