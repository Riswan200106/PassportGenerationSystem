using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For session handling
using PassportGenerationSystem.DAL;
using PassportGenerationSystem.Models;

namespace PassportGenerationSystem.Controllers
{
    public class DefaultController : Controller
    {
        private readonly Account_DAL accountDal;

        public DefaultController()
        {
            accountDal = new Account_DAL();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: CreateUser page (Signup)
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: Handle the CreateUser (Signup)
        [HttpPost]
        public IActionResult CreateUser(Accounts signup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    signup.Role = "User";
                    accountDal.Signup(signup);
                    TempData["SuccessMessage"] = "Signup successful.";
                    return RedirectToAction("SignIn");
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while creating user: " + ex.Message;
            }
            return View(signup);
        }

        // GET: SignIn page
        public IActionResult SignIn()
        {
            return View();
        }

        // POST: Handle SignIn form submission
        [HttpPost]
        public IActionResult SignIn(string Username, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check for default admin credentials
                    if (Username == "admin" && Password == "admin123")
                    {
                        HttpContext.Session.SetInt32("UserId", 0); // Default admin ID
                        HttpContext.Session.SetString("Username", "Admin");
                        HttpContext.Session.SetString("Role", "Admin");

                        TempData["SuccessMessage"] = "Welcome, Admin!";
                        return RedirectToAction("AdminDashboard", "Admin");
                    }

                    // Check for admin or user credentials in the database
                    var user = accountDal.Signin(Username, Password);
                    if (user != null)
                    {
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        HttpContext.Session.SetString("Username", user.FirstName);
                        HttpContext.Session.SetString("Role", user.Role);

                        if (user.Role == "Admin")
                        {
                            TempData["SuccessMessage"] = $"Welcome, Admin {user.FirstName}!";
                            return RedirectToAction("AdminDashboard", "Admin");
                        }
                        else if (user.Role == "User")
                        {
                            TempData["SuccessMessage"] = $"Welcome, {user.FirstName}!";
                            return RedirectToAction("UserDashboard", "User");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid role.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid username or password.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error during sign-in: " + ex.Message;
            }

            return View();
        }



        // Logout and clear the session
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have successfully logged out.";
            return RedirectToAction("Index");
        }
    }
}
