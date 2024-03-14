using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotPix.Migrations
{
    /// <inheritdoc />
    public partial class AddPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", nullable: true),
                    status = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    account_origin_id = table.Column<int>(type: "integer", nullable: false),
                    pix_key_destiny_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_payment_provider_accounts_account_origin_id",
                        column: x => x.account_origin_id,
                        principalTable: "payment_provider_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_pix_keys_pix_key_destiny_id",
                        column: x => x.pix_key_destiny_id,
                        principalTable: "pix_keys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_account_origin_id",
                table: "payments",
                column: "account_origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_pix_key_destiny_id",
                table: "payments",
                column: "pix_key_destiny_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");
        }
    }
}
