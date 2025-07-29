using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ChatUapp.Migrations
{
    /// <inheritdoc />
    public partial class Add_Chatbot_Management_Bounded_Context : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bot");

            migrationBuilder.RenameTable(
                name: "TenantChatbotUsers",
                schema: "tnt",
                newName: "TenantChatbotUsers",
                newSchema: "bot");

            migrationBuilder.RenameTable(
                name: "Messages",
                schema: "msg",
                newName: "Messages",
                newSchema: "bot");

            migrationBuilder.AlterColumn<int>(
                name: "MessageType",
                schema: "bot",
                table: "Messages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.CreateTable(
                name: "ChatBots",
                schema: "bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Header = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SubHeader = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    UniqueKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BrandImageName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IconName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IconColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatBots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatBots_AbpTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AbpTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatSesions",
                schema: "bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionCreator = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatbotId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    BrowserSessionKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSesions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatSesions_AbpTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AbpTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatSesions_ChatBots_ChatbotId",
                        column: x => x.ChatbotId,
                        principalSchema: "bot",
                        principalTable: "ChatBots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSources",
                schema: "bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatbotId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Origin_SourceType = table.Column<int>(type: "integer", nullable: false),
                    Origin_SourceUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Origin_FileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Origin_FileType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Origin_TextContent = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSources_ChatBots_ChatbotId",
                        column: x => x.ChatbotId,
                        principalSchema: "bot",
                        principalTable: "ChatBots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                schema: "bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageText = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatSesions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "bot",
                        principalTable: "ChatSesions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantChatbotUsers_ChatbotId",
                schema: "bot",
                table: "TenantChatbotUsers",
                column: "ChatbotId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantChatbotUsers_TenantId",
                schema: "bot",
                table: "TenantChatbotUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBots_TenantId",
                schema: "bot",
                table: "ChatBots",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBots_UniqueKey",
                schema: "bot",
                table: "ChatBots",
                column: "UniqueKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SentAt",
                schema: "bot",
                table: "ChatMessages",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SessionId",
                schema: "bot",
                table: "ChatMessages",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSesions_ChatbotId",
                schema: "bot",
                table: "ChatSesions",
                column: "ChatbotId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSesions_SessionCreator",
                schema: "bot",
                table: "ChatSesions",
                column: "SessionCreator");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSesions_TenantId",
                schema: "bot",
                table: "ChatSesions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSources_ChatbotId",
                schema: "bot",
                table: "TrainingSources",
                column: "ChatbotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantChatbotUsers_AbpTenants_TenantId",
                schema: "bot",
                table: "TenantChatbotUsers",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantChatbotUsers_ChatBots_ChatbotId",
                schema: "bot",
                table: "TenantChatbotUsers",
                column: "ChatbotId",
                principalSchema: "bot",
                principalTable: "ChatBots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantChatbotUsers_AbpTenants_TenantId",
                schema: "bot",
                table: "TenantChatbotUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantChatbotUsers_ChatBots_ChatbotId",
                schema: "bot",
                table: "TenantChatbotUsers");

            migrationBuilder.DropTable(
                name: "ChatMessages",
                schema: "bot");

            migrationBuilder.DropTable(
                name: "TrainingSources",
                schema: "bot");

            migrationBuilder.DropTable(
                name: "ChatSesions",
                schema: "bot");

            migrationBuilder.DropTable(
                name: "ChatBots",
                schema: "bot");

            migrationBuilder.DropIndex(
                name: "IX_TenantChatbotUsers_ChatbotId",
                schema: "bot",
                table: "TenantChatbotUsers");

            migrationBuilder.DropIndex(
                name: "IX_TenantChatbotUsers_TenantId",
                schema: "bot",
                table: "TenantChatbotUsers");

            migrationBuilder.EnsureSchema(
                name: "msg");

            migrationBuilder.RenameTable(
                name: "TenantChatbotUsers",
                schema: "bot",
                newName: "TenantChatbotUsers",
                newSchema: "tnt");

            migrationBuilder.RenameTable(
                name: "Messages",
                schema: "bot",
                newName: "Messages",
                newSchema: "msg");

            migrationBuilder.AlterColumn<byte>(
                name: "MessageType",
                schema: "msg",
                table: "Messages",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
