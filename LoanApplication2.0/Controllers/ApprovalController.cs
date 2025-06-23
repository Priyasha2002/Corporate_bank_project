

using System;
using System.Linq;
using LoanApplicationSystem2._0.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplicationSystem.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Details(int applicationId)
        {
            var application = _context.LoanApplications.FirstOrDefault(x => x.Id == applicationId);
            if (application == null)
                return NotFound();

            var approvals = _context.Approvals
                .Where(a => a.ApplicationId == applicationId)
                .OrderByDescending(a => a.ApprovalDate)
                .ToList();

            // Optionally pass latest approval info for editing
            var latestApproval = approvals.FirstOrDefault();
            ViewBag.Approvals = approvals;
            ViewBag.LatestApproverId = latestApproval?.ApproverId.ToString() ?? "";
            ViewBag.LatestComments = latestApproval?.Comments ?? "";

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(int id, string loanStatus, string ApproverId, string Comments)
        {
            var application = _context.LoanApplications.FirstOrDefault(x => x.Id == id);
            if (application == null)
                return NotFound();

            if (application.Status == ApplicationStatus.Pending)
            {
                if (loanStatus == "Approved")
                    application.Status = ApplicationStatus.Approved;
                else if (loanStatus == "Rejected")
                    application.Status = ApplicationStatus.Rejected;

                int approverIdInt = 0;
                int.TryParse(ApproverId, out approverIdInt);

                var approval = new Approval
                {
                    ApplicationId = application.Id,
                    ApprovalStatus = (ApprovalStatus)application.Status,
                    ApprovalDate = DateTime.Now,
                    ApproverId = approverIdInt,
                    ApprovalLevel = 1,
                    Comments = Comments ?? ""
                };
                _context.Approvals.Add(approval);

                _context.SaveChanges();
                TempData["Message"] = $"Loan application has been {loanStatus.ToLower()}.";
            }
            else
            {
                TempData["Message"] = "This loan application has already been decided and cannot be changed.";
            }

            return RedirectToAction("Details", new { applicationId = id });
        }
    }
}
