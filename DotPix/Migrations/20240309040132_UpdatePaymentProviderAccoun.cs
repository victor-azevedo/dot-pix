using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotPix.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentProviderAccoun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_cpf",
                table: "users",
                column: "cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_cpf",
                table: "users");
        }
    }
}
