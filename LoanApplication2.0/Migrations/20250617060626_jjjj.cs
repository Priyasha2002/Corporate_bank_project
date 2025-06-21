using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplication2._0.Migrations
{
    /// <inheritdoc />
    public partial class jjjj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disbursements_LoanApplications_ApplicationId",
                table: "Disbursements");

            migrationBuilder.DropIndex(
                name: "IX_Disbursements_ApplicationId",
                table: "Disbursements");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Disbursements",
                newName: "LoanApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanApplicationId",
                table: "Disbursements",
                newName: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_ApplicationId",
                table: "Disbursements",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disbursements_LoanApplications_ApplicationId",
                table: "Disbursements",
                column: "ApplicationId",
                principalTable: "LoanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
