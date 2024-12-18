using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
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

        /// <summary>
        /// Displays the "About" page.
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Displays the "Contact" page.
        /// </summary>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the "CreateUser" (Signup) page.
        /// </summary>
        public IActionResult CreateUser()
        {
            return View();
        }

        /// <summary>
        /// Handles the signup process for a new user.
        /// </summary>
        /// <param name="signup">The user's signup details.</param>
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

        /// <summary>
        /// Displays the "SignIn" page.
        /// </summary>
        public IActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Handles the sign-in process for an admin or user.
        /// </summary>
        /// <param name="Username">The username entered by the user.</param>
        /// <param name="Password">The password entered by the user.</param>
        [HttpPost]
        public IActionResult SignIn(string Username, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Username == "adminmain" && Password == "Admin@123")
                    {
                        HttpContext.Session.SetInt32("UserId", 0); 
                        HttpContext.Session.SetString("Username", "Admin");
                        HttpContext.Session.SetString("Role", "Admin");

                        TempData["SuccessMessage"] = "Welcome, Admin!";
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
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

        /// <summary>
        /// Logs the user out by clearing the session.
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have successfully logged out.";
            return RedirectToAction("Index");
        }
    }
}
