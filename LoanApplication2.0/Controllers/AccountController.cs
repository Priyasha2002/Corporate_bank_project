
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public ActionResult Login()
    {
        // You can return a login view or a simple message for now
        return View();
    }
}