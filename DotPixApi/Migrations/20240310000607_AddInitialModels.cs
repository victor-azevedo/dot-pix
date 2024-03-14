using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotPixApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_providers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    cpf = table.Column<string>(type: "varchar(11)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_provider_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payment_provider_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_provider_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_provider_tokens_payment_providers_payment_provider_~",
                        column: x => x.payment_provider_id,
                        principalTable: "payment_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment_provider_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account = table.Column<string>(type: "varchar(20)", nullable: false),
                    agency = table.Column<string>(type: "varchar(20)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payment_provider_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_provider_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_provider_accounts_payment_providers_payment_provide~",
                        column: x => x.payment_provider_id,
                        principalTable: "payment_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payment_provider_accounts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pix_keys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "varchar(10)", nullable: false),
                    value = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payment_provider_account_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pix_keys", x => x.id);
                    table.ForeignKey(
                        name: "FK_pix_keys_payment_provider_accounts_payment_provider_account~",
                        column: x => x.payment_provider_account_id,
                        principalTable: "payment_provider_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payment_provider_accounts_payment_provider_id",
                table: "payment_provider_accounts",
                column: "payment_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_provider_accounts_user_id",
                table: "payment_provider_accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_provider_tokens_payment_provider_id",
                table: "payment_provider_tokens",
                column: "payment_provider_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_provider_tokens_token",
                table: "payment_provider_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pix_keys_payment_provider_account_id",
                table: "pix_keys",
                column: "payment_provider_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_cpf",
                table: "users",
                column: "cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_provider_tokens");

            migrationBuilder.DropTable(
                name: "pix_keys");

            migrationBuilder.DropTable(
                name: "payment_provider_accounts");

            migrationBuilder.DropTable(
                name: "payment_providers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
