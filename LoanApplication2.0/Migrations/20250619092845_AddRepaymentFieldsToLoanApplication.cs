using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplication2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddRepaymentFieldsToLoanApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // COMMENT THIS OUT for this specific run, as the column already exists
            // migrationBuilder.AddColumn<int>(
            //     name: "RepaymentDuration",
            //     table: "LoanApplications",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateOfRepayment", // Correct: No spaces
                table: "LoanApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepaymentDuration",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "StartDateOfRepayment",
                table: "LoanApplications");
        }
    }
}