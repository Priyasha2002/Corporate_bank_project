

using System;
using System.ComponentModel.DataAnnotations;

namespace LoanApplicationSystem2._0.Models
{
    public class LoanTrackingViewModel
    {
        public int ApplicationId { get; set; }
        public string CompanyName { get; set; }
        public string LoanType { get; set; }
        public decimal LoanAmount { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime SubmissionDate { get; set; }
        public string LatestApprovalStatus { get; set; }
        public string GSTIN { get; set; }

        
        public string Industry { get; set; }

        
        public int YearsInOperation { get; set; }

        
       

        
        public string LoanPurpose { get; set; }

       
        public decimal Turnover { get; set; }

       
        public decimal NetProfit { get; set; }

        
        public decimal ExistingDebt { get; set; }

        public string Collateral { get; set; }

    }
}



