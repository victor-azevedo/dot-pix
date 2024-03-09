using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotPix.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentProviderTokenIndexUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_payment_provider_tokens_token",
                table: "payment_provider_tokens",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_payment_provider_tokens_token",
                table: "payment_provider_tokens");
        }
    }
}
