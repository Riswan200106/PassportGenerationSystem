using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using PassportGenerationSystem.DAL;
using PassportGenerationSystem.Models;
using PassportGenerationSystem.EncryptHelper;

namespace PassportGenerationSystem.Controllers
{
    public class DefaultController : Controller
    {
        private readonly Account_DAL account_Dal;

        /// <summary>
        /// Constructor to initialize DefaultController with a DAL instance using Dependency Injection.
        /// </summary>
        /// <param name="accountDal">Instance of Account_DAL provided by the DI container.</param>
        public DefaultController(Account_DAL accountDal)
        {
            this.account_Dal = accountDal; 
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


        [HttpPost]
        public IActionResult SubmitFeedback(Feedback feedback)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    account_Dal.AddFeedback(feedback); 
                    TempData["SuccessMessage"] = "Thank you for your feedback!";
                    return RedirectToAction("Contact");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return View("Contact");
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
                    signup.Password = EncryptionHelper.EncryptPassword(signup.Password);
                    string resultMessage = account_Dal.Signup(signup);

                    if (resultMessage == "Signup successful.")
                    {
                        TempData["SuccessMessage"] = resultMessage;
                        return RedirectToAction("SignIn");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = resultMessage;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
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
                    var user = account_Dal.Signin(Username);

                    if (user != null)
                    {
                        string encryptedPassword = EncryptionHelper.EncryptPassword(Password);
                        if (user.Password == encryptedPassword)
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
                                TempData["SuccessMessage"] = $"Welcome, {user.FirstName + user.LastName}!";
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
                ErrorLogger.LogError(ex);
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
