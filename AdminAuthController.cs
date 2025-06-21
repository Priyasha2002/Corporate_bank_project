namespace LoanApplication2._0.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        // ... other code ...

        // Place the Welcome action here
        public IActionResult Welcome()
        {
            return View();
        }

        // ... other actions ...
    }
}
