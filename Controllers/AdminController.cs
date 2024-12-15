using PassportGenerationSystem.DAL;
using PassportGenerationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PassportGenerationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly Account_DAL accountDal;

        public AdminController()
        {
            accountDal = new Account_DAL();
        }
        // Display Admin Dashboard
        public IActionResult AdminDashboard()
        {

            return View();

        }


        // Display the list of all users
        public IActionResult ViewUsers()
        {
            List<Accounts> users = new List<Accounts>();

            try
            {
                users = accountDal.GetAllUsers();
                if (users.Count == 0)
                {
                    TempData["ErrorMessage"] = "No users found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching users: " + ex.Message;
            }

            return View(users);
        }



        // GET: Display Delete Confirmation
        public ActionResult Delete(int id)
        {
            var user = accountDal.GetAllUsers().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ViewUsers");
            }

            return View(user); // Pass the user details to confirmation view
        }

        // POST: Handle the confirmed deletion
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool isDeleted = accountDal.DeleteUser(id);

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "User deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to delete the user.";
                }

                return RedirectToAction("ViewUsers");
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = "An error occurred: " + exception.Message;
                return RedirectToAction("ViewUsers");
            }
        }


        public IActionResult AddNewAdmin()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddNewAdmin(Accounts admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    admin.Role = "Admin";
                    accountDal.AddNewAdmin(admin);
                    TempData["SuccessMessage"] = "Admin account created successfully.";
                    return RedirectToAction("SignIn","Default");
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while adding new admin: " + ex.Message;
            }

            return View(admin);
        }

    }
}
