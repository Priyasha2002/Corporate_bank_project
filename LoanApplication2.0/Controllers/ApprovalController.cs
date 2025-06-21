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

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(int id, string loanStatus)
        {
            var application = _context.LoanApplications.FirstOrDefault(x => x.Id == id);
            if (application == null)
                return NotFound();

            if (loanStatus == "Approved")
                application.Status = ApplicationStatus.Approved;
            else if (loanStatus == "Rejected")
                application.Status = ApplicationStatus.Rejected;

            _context.SaveChanges();
            TempData["Message"] = $"Loan application has been {loanStatus.ToLower()}.";

            return RedirectToAction("Details", new { applicationId = id });
        }
    }
}