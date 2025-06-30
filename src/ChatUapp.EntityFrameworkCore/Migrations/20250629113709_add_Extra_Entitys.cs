using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatUapp.Migrations
{
    /// <inheritdoc />
    public partial class add_Extra_Entitys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUsers");

            migrationBuilder.RenameColumn(
                name: "TwitterUrl",
                table: "AbpUsers",
                newName: "twitterUrl");

            migrationBuilder.RenameColumn(
                name: "TitlePrefix",
                table: "AbpUsers",
                newName: "titlePrefix");

            migrationBuilder.RenameColumn(
                name: "LinkedInUrl",
                table: "AbpUsers",
                newName: "linkedInUrl");

            migrationBuilder.RenameColumn(
                name: "InstagramUrl",
                table: "AbpUsers",
                newName: "instagramUrl");

            migrationBuilder.RenameColumn(
                name: "FacebookUrl",
                table: "AbpUsers",
                newName: "facebookUrl");

            migrationBuilder.AlterColumn<string>(
                name: "twitterUrl",
                table: "AbpUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "linkedInUrl",
                table: "AbpUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "instagramUrl",
                table: "AbpUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "facebookUrl",
                table: "AbpUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "twitterUrl",
                table: "AbpUsers",
                newName: "TwitterUrl");

            migrationBuilder.RenameColumn(
                name: "titlePrefix",
                table: "AbpUsers",
                newName: "TitlePrefix");

            migrationBuilder.RenameColumn(
                name: "linkedInUrl",
                table: "AbpUsers",
                newName: "LinkedInUrl");

            migrationBuilder.RenameColumn(
                name: "instagramUrl",
                table: "AbpUsers",
                newName: "InstagramUrl");

            migrationBuilder.RenameColumn(
                name: "facebookUrl",
                table: "AbpUsers",
                newName: "FacebookUrl");

            migrationBuilder.AlterColumn<string>(
                name: "TwitterUrl",
                table: "AbpUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedInUrl",
                table: "AbpUsers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InstagramUrl",
                table: "AbpUsers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FacebookUrl",
                table: "AbpUsers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUsers",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");
        }
    }
}
