using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpinaBets.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IDNumber",
                table: "AspNetUsers",
                column: "IDNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IDNumber",
                table: "AspNetUsers");
        }
    }
}
