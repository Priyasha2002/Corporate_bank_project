// Controllers/LoanController.cs
using System;
using System.IO;
using System.Linq;
using LoanApplicationSystem2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting; // Add this for IWebHostEnvironment

namespace LoanApplicationSystem2._0.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment; // Inject IWebHostEnvironment

        public LoanController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment; // Initialize it
        }

        public IActionResult DashboardWithApply()
        {
            string clientId = HttpContext.Session.GetString("ClientId");
            bool hasLoans = false;

            if (!string.IsNullOrEmpty(clientId))
            {
                hasLoans = _db.LoanApplications.Any(l => l.ClientId == clientId);
            }

            ViewBag.HasLoans = hasLoans;
            return View();
        }

        public IActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(LoanApplication model) // Made async for file operations
        {
            string clientId = HttpContext.Session.GetString("ClientId");
            if (string.IsNullOrEmpty(clientId))
            {
                return RedirectToAction("LoginC", "ClientAuth");
            }

            model.ClientId = clientId;
            model.SubmissionDate = DateTime.Now;

            // Manual validation for CompanyDocument if needed, or rely on client-side 'required'
            if (model.CompanyDocument == null || model.CompanyDocument.Length == 0)
            {
                ModelState.AddModelError("CompanyDocument", "Please upload a company document.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToList();

                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {error.Key}");
                    foreach (var e in error.Errors)
                    {
                        Console.WriteLine($"  Error: {e.ErrorMessage}");
                    }
                }
                return View(model);
            }

            // Save LoanApplication first to get its ID
            _db.LoanApplications.Add(model);
            _db.SaveChanges(); // Save changes to get the auto-generated ApplicationId

            // Handle file upload
            if (model.CompanyDocument != null && model.CompanyDocument.Length > 0)
            {
                try
                {
                    // Define upload folder path
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documents");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Create a unique file name to prevent conflicts
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CompanyDocument.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CompanyDocument.CopyToAsync(fileStream);
                    }

                    // Save document details to LoanDocuments table
                    var loanDocument = new LoanDocument
                    {
                        ApplicationId = model.Id, // Use the ID of the newly saved LoanApplication
                        DocumentType = "Company Document", // Or infer from file type, or add a field to form
                        FilePath = "/documents/" + uniqueFileName, // Store relative path for web access
                        UploadDate = DateTime.Now,
                        IsValid = true // Set initial validity
                    };
                    _db.LoanDocuments.Add(loanDocument);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the exception (e.g., using ILogger)
                    Console.WriteLine($"Error uploading file: {ex.Message}");
                    ModelState.AddModelError("CompanyDocument", "Error uploading document. Please try again.");
                    // You might want to delete the loan application if document upload fails,
                    // or handle this more gracefully. For now, we'll let it proceed.
                    return View(model); // Return to view with error
                }
            }
            else
            {
                // This case should ideally be caught by the initial ModelState.AddModelError above.
                // However, it's good to have a fallback or specific message if somehow it's missed.
                // If the document is truly optional, you can remove this else block.
                ModelState.AddModelError("CompanyDocument", "No document was uploaded.");
                return View(model);
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult AdminList()
        {
            var applications = _db.LoanApplications.ToList();
            var latestApprovals = _db.Approvals
                .GroupBy(a => a.ApplicationId)
                .Select(g => new
                {
                    ApplicationId = g.Key,
                    LatestStatus = g.OrderByDescending(a => a.ApprovalDate).FirstOrDefault().ApprovalStatus.ToString()
                }).ToList();

            var model = _db.LoanApplications.Select(app => new LoanTrackingViewModel
            {
                ApplicationId = app.Id,
                CompanyName = app.CompanyName,
                LoanType = app.LoanPurpose,
                LoanAmount = app.LoanAmount,
                Status = app.Status,
                SubmissionDate = app.SubmissionDate,
                LatestApprovalStatus = app.Status.ToString() // Always up-to-date!
            }).ToList();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int ApplicationId, ApplicationStatus Status)
        {
            var application = _db.LoanApplications.FirstOrDefault(a => a.Id == ApplicationId);
            if (application != null)
            {
                application.Status = Status;
                _db.SaveChanges();
                TempData["Message"] = "Status updated successfully!";
            }
            return RedirectToAction("AdminList");
        }

        public IActionResult CreditScore()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCreditScores()
        {
            var loanApplications = _db.LoanApplications
                .OrderByDescending(x => x.SubmissionDate)
                .ToList();

            var list = loanApplications.Select(x => new
            {
                x.Id,
                x.CompanyName,
                SubmissionDate = x.SubmissionDate,
                CreditScore = CalculateCreditScore(x.ExistingDebt, x.Turnover, x.NetProfit)
            }).ToList();

            return Json(list);
        }

        private int CalculateCreditScore(decimal existingDebt, decimal turnover, decimal netProfit)
        {
            decimal debtToTurnover = (turnover > 0) ? existingDebt / turnover : 1m;
            decimal profitMargin = (turnover > 0) ? netProfit / turnover : 0m;

            decimal debtScore = (debtToTurnover >= 1m) ? 0m : (1m - debtToTurnover) * 100m;
            if (debtScore < 0m) debtScore = 0m;

            decimal profitScore = (profitMargin <= 0m) ? 0m : (profitMargin >= 0.3m ? 100m : (profitMargin / 0.3m) * 100m);

            decimal creditScore = (debtScore * 0.6m) + (profitScore * 0.4m);
            if (creditScore < 1m) creditScore = 1m;
            if (creditScore > 100m) creditScore = 100m;

            return (int)Math.Round(creditScore);
        }

        public IActionResult Documents(int applicationId)
        {
            var documents = _db.LoanDocuments
                               .Where(d => d.ApplicationId == applicationId)
                               .ToList();

            ViewBag.ApplicationId = applicationId;
            return View(documents);
        }
        public IActionResult Support()
        {
            return View();
        }
    }
}