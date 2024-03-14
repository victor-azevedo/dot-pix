using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotPixApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexUniquePixKeyValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_pix_keys_value",
                table: "pix_keys",
                column: "value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pix_keys_value",
                table: "pix_keys");
        }
    }
}
