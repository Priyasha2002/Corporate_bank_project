// Models/ClientLoanStatusViewModel.cs
public class ClientLoanStatusViewModel
{
    public int LoanId { get; set; }
    public string CompanyName { get; set; }
    public decimal LoanAmount { get; set; }
    public string LoanPurpose { get; set; }
    public string LatestApprovalStatus { get; set; }
    public System.DateTime SubmissionDate { get; set; }

    // Add these for disbursement details
    public decimal? DisbursedAmount { get; set; }
    public System.DateTime? DisbursementDate { get; set; }

    // Removed: public string RepaymentSchedule { get; set; } // This line is GONE

    // Corrected: RepaymentDuration should be an int to match LoanApplication.RepaymentDuration
    public int RepaymentDuration { get; set; }
}