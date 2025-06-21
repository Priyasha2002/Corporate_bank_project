using System;
using LoanApplicationSystem2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace LoanApplication2._0.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        // List of allowed admin Employee IDs
        private static readonly HashSet<string> AllowedAdminIds = new HashSet<string>
        {
            "2406695",
            "2406689",
            "2406688",
            "2406633"
        };

        private const string AdminPassword = "AdminLogin";

        public AdminAuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult LoginA()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // If using authentication, sign out here
            // await HttpContext.SignOutAsync(); // Uncomment if using authentication

            // Redirect to welcome page
            return RedirectToAction("Welcome", "AdminAuth");
        }

        [HttpPost]
        public IActionResult Login(string EmployeeId, string Password, bool? KeepLoggedIn)
        {
            // Only allow login for the specified Employee IDs and password
            if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(Password)
                && AllowedAdminIds.Contains(EmployeeId) && Password == AdminPassword)
            {
                // Optionally, set session/cookie here for logged-in admin
                HttpContext.Session.SetString("AdminId", EmployeeId);

                // Record login in the database
                var loginRecord = new AdminLoginRecord
                {
                    EmployeeId = EmployeeId,
                    LoginTime = DateTime.Now,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                _db.AdminLoginRecords.Add(loginRecord);
                _db.SaveChanges();

                // Redirect to the admin dashboard (AdminList in LoanController)
                return RedirectToAction("AdminList", "Loan");
            }

            ViewBag.Error = "Invalid Employee ID or Password.";
            return View("LoginA");
        }
    }
}