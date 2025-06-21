using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplication2._0.Migrations
{
    /// <inheritdoc />
    public partial class RenameApplicationIdToIdInDisbursements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disbursements_LoanApplications_ApplicationId",
                table: "Disbursements");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Disbursements",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Disbursements_ApplicationId",
                table: "Disbursements",
                newName: "IX_Disbursements_Id");

            migrationBuilder.AlterColumn<string>(
                name: "RepaymentSchedule",
                table: "Disbursements",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Disbursements_LoanApplications_Id",
                table: "Disbursements",
                column: "Id",
                principalTable: "LoanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disbursements_LoanApplications_Id",
                table: "Disbursements");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Disbursements",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Disbursements_Id",
                table: "Disbursements",
                newName: "IX_Disbursements_ApplicationId");

            migrationBuilder.AlterColumn<string>(
                name: "RepaymentSchedule",
                table: "Disbursements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

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
