// Models/LoanApplication.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace LoanApplicationSystem2._0.Models
{
    public class LoanApplication
    {
        [Key]
        public int Id { get; set; }

        public string? ClientId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Business Type is required.")]
        public string BusinessType { get; set; }

        [Required(ErrorMessage = "Registration Number is required.")]
        [StringLength(50)]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "GSTIN is required.")]
        [StringLength(15)]
        public string GSTIN { get; set; }

        [Required(ErrorMessage = "Industry is required.")]
        public string Industry { get; set; }

        [Range(0, 100, ErrorMessage = "Years in Operation must be between 0 and 100.")]
        public int YearsInOperation { get; set; }

        [Required(ErrorMessage = "Loan Amount is required.")]
        [Range(1000, 1000000000, ErrorMessage = "Loan amount must be between 1,000 and 1,000,000,000.")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Loan Purpose is required.")]
        [StringLength(500)]
        public string LoanPurpose { get; set; }

        [Required(ErrorMessage = "Turnover is required.")]
        [Range(0, 1000000000000, ErrorMessage = "Turnover must be a positive value.")]
        public decimal Turnover { get; set; }

        [Required(ErrorMessage = "Net Profit is required.")]
        public decimal NetProfit { get; set; }

        [Required(ErrorMessage = "Existing Debt is required.")]
        public decimal ExistingDebt { get; set; }

        public string Collateral { get; set; }

        // NEW FIELDS
        [Required(ErrorMessage = "Start Date of Repayment is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDateOfRepayment { get; set; }

        [Required(ErrorMessage = "Repayment Duration is required.")]
        [Range(1, 360, ErrorMessage = "Repayment duration must be between 1 and 360 months.")]
        public int RepaymentDuration { get; set; } // in months

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        [NotMapped]
        public IFormFile CompanyDocument { get; set; }
    }

    public enum ApplicationStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
