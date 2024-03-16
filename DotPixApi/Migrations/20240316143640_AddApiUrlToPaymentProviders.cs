using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotPixApi.Migrations
{
    /// <inheritdoc />
    public partial class AddApiUrlToPaymentProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "api_url",
                table: "payment_providers",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "api_url",
                table: "payment_providers");
        }
    }
}
