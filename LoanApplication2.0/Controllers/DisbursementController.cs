using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoanApplication2._0.Models;
using LoanApplication2._0;
using LoanApplicationSystem2._0.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication2._0.Controllers
{
    public class DisbursementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisbursementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult TrackDisbursement()
        {
            // Populate dropdown with LoanApplicationId and CompanyName
            var applications = _context.LoanApplications
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Id} - {a.CompanyName}"
                }).ToList();

            ViewBag.LoanApplications = applications;

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult TrackDisbursement(Disbursement disbursement)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        _context.Disbursements.Add(disbursement);
        //        _context.SaveChanges();
        //        return RedirectToAction("Tracking", "Disbursement");
        //    }

        //    // Repopulate dropdown if model state is invalid
        //    ViewBag.LoanApplications = _context.LoanApplications
        //        .Select(a => new SelectListItem
        //        {
        //            Value = a.Id.ToString(),
        //            Text = $"{a.Id} - {a.CompanyName}"
        //        }).ToList();
        //    return View(disbursement);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TrackDisbursement(Disbursement disbursement)
        {
            if (ModelState.IsValid)
            {
                // Fetch the selected LoanApplication from the database
                var loanApplication = _context.LoanApplications
                    .FirstOrDefault(a => a.Id == disbursement.LoanApplicationId);

               
                if (loanApplication != null)
                {
                    // Set the DisbursementAmount from the LoanApplication's LoanAmount
                    disbursement.DisbursedAmount = loanApplication.LoanAmount;
                    disbursement.RepaymentSchedule = loanApplication.RepaymentDuration;
                    _context.Disbursements.Add(disbursement);
                    _context.SaveChanges();

                    return RedirectToAction("Tracking", "Disbursement");
                }

                ModelState.AddModelError("", "Selected loan application not found.");
            }

            // Repopulate dropdown if model state is invalid
            ViewBag.LoanApplications = _context.LoanApplications
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Id} - {a.CompanyName}"
                }).ToList();

            return View(disbursement);
        }


        public IActionResult Tracking()
        {
            var disbursements = _context.Disbursements.ToList();
            return View(disbursements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var disbursement = _context.Disbursements.Find(id);
            if (disbursement != null)
            {
                _context.Disbursements.Remove(disbursement);
                _context.SaveChanges();
            }
            return RedirectToAction("Tracking");
        }
    }
}
