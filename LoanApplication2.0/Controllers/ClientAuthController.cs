using System.Linq;
using LoanApplicationSystem2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LoanApplication2._0.Controllers
{
    public class ClientAuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClientAuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /ClientAuth/LoginC
        public IActionResult LoginC()
        {
            return View();
        }

        // POST: /ClientAuth/LoginC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginC(string LoginId, string Password, bool? KeepLoggedIn)
        {
            if (string.IsNullOrEmpty(LoginId) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("", "Application ID or Email and Password are required.");
                return View();
            }

            // Try to find by ApplicationId or Email
            var client = _db.Clients
                .FirstOrDefault(c =>
                    (c.ApplicationId.ToString() == LoginId || c.Email == LoginId)
                    && c.Password == Password);

            if (client != null)
            {
                // Set session
                HttpContext.Session.SetString("ClientId", client.ApplicationId.ToString());

                // Optionally, record login
                var loginRecord = new ClientLoginRecord
                {
                    ApplicationId = client.ApplicationId.ToString(),
                    LoginTime = System.DateTime.Now,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                _db.ClientLoginRecords.Add(loginRecord);
                _db.SaveChanges();

                // Redirect to dashboard with apply
                return RedirectToAction("DashboardWithApply", "Loan");
            }

            ModelState.AddModelError("", "Invalid Application ID/Email or Password.");
            return View();
        }

        // GET: /ClientAuth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /ClientAuth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(ClientRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if email already exists
            if (_db.Clients.Any(c => c.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(model);
            }

            // Check if passwords match
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            // Create and save new client
            var client = new Client
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };
            _db.Clients.Add(client);
            _db.SaveChanges();

            // Redirect to a registration success page (create RegisterSuccess.cshtml if needed)
            return RedirectToAction("RegisterSuccess");
        }

        // GET: /ClientAuth/RegisterSuccess
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        // GET: /ClientAuth/Logout
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Redirect to welcome page
            return RedirectToAction("Welcome", "AdminAuth");
        }

        // GET: /ClientAuth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /ClientAuth/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string Email, string Name)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("", "Email and Name are required.");
                return View();
            }

            var client = _db.Clients.FirstOrDefault(c => c.Email == Email && c.Name == Name);
            if (client == null)
            {
                ModelState.AddModelError("", "No user found with the provided details.");
                return View();
            }

            // For demo: Show password on screen (not secure for production)
            ViewBag.RecoveredPassword = client.Password;
            return View();
        }

        // (Other actions, e.g., ForgotPassword, remain unchanged)
    }
}
