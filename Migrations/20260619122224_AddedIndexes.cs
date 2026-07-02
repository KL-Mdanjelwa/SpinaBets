using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpinaBets.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionDate",
                table: "Transactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_Status",
                table: "Bets",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionDate",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Bets_Status",
                table: "Bets");
        }
    }
}
