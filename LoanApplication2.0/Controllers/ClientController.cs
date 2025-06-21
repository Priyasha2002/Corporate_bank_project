// Controllers/ClientController.cs
using System;
using System.Linq;
using LoanApplicationSystem2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic; // Make sure this is included for Select

namespace LoanApplication2._0.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClientController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult MyLoans()
        {
            string clientId = HttpContext.Session.GetString("ClientId");

            if (string.IsNullOrEmpty(clientId))
            {
                // Not logged in or session expired, redirect to login
                return RedirectToAction("LoginC", "ClientAuth");
            }

            var loans = _db.LoanApplications
                .Where(l => l.ClientId == clientId)
                .ToList();

            // The 'approvals' query is currently not used for the *main* status display
            // If it's for historical approval steps, you might keep it, but for the primary status,
            // we will use loan.Status directly.
            // Keeping it for now as it's in your provided code, even if not directly used in the ViewModel mapping below.
            var approvals = _db.Approvals
                .GroupBy(a => a.ApplicationId)
                .Select(g => new
                {
                    ApplicationId = g.Key,
                    LatestStatus = g.OrderByDescending(a => a.ApprovalDate).FirstOrDefault().ApprovalStatus.ToString()
                }).ToList();

            var disbursements = _db.Disbursements.ToList();

            var model = loans.Select(loan =>
            {
                // Use ApplicationId as the foreign key for Disbursement
                var disb = disbursements.FirstOrDefault(d => d.LoanApplicationId == loan.Id);

                return new ClientLoanStatusViewModel
                {
                    LoanId = loan.Id,
                    CompanyName = loan.CompanyName,
                    LoanAmount = loan.LoanAmount,
                    LoanPurpose = loan.LoanPurpose,
                    SubmissionDate = loan.SubmissionDate,
                    // Get the status directly from the LoanApplication object
                    LatestApprovalStatus = loan.Status.ToString(), // This is the definitive status!

                    DisbursedAmount = disb?.DisbursedAmount,
                    DisbursementDate = disb?.DisbursementDate,
                    // Populate RepaymentDuration from the LoanApplication (it's an int)
                    RepaymentDuration = loan.RepaymentDuration // Directly use the int value from LoanApplication
                    // Removed: RepaymentSchedule = disb?.RepaymentSchedule // This line is GONE
                };
            }).ToList();

            return View(model);
        }

        // Your other actions (CreateLoan, AdminList, UpdateStatus, GetCreditScores, Documents, Support) remain unchanged
        // For brevity, I'm not including them here, but ensure they are in your actual ClientController.cs file.
    }
}