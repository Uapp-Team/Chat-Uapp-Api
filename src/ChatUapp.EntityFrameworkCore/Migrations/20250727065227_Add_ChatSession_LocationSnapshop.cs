using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatUapp.Migrations
{
    /// <inheritdoc />
    public partial class Add_ChatSession_LocationSnapshop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ip",
                schema: "bot",
                table: "ChatSesions",
                newName: "IpAddress");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                schema: "bot",
                table: "ChatSesions",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                schema: "bot",
                table: "ChatSesions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Flag",
                schema: "bot",
                table: "ChatSesions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "bot",
                table: "ChatSesions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "bot",
                table: "ChatSesions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryName",
                schema: "bot",
                table: "ChatSesions");

            migrationBuilder.DropColumn(
                name: "Flag",
                schema: "bot",
                table: "ChatSesions");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "bot",
                table: "ChatSesions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "bot",
                table: "ChatSesions");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                schema: "bot",
                table: "ChatSesions",
                newName: "Ip");

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                schema: "bot",
                table: "ChatSesions",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45);
        }
    }
}
