﻿using PassportGenerationSystem.DAL;
using PassportGenerationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PassportGenerationSystem.EncryptHelper;

namespace PassportGenerationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly Account_DAL account_Dal;
        private readonly User_DAL user_Dal;

        /// <summary>
        /// Initializes a new instance of the AdminController class.
        /// Uses constructor injection to receive instances of Account_DAL and User_DAL.
        /// These dependencies are assigned to the class fields for use in the controller's actions.
        /// </summary>
        /// <param name="accountDal">The instance of Account_DAL to be used by the controller.</param>
        /// <param name="userDal">The instance of User_DAL to be used by the controller.</param>
        public AdminController(Account_DAL accountDal, User_DAL userDal)
        {
            this.account_Dal = accountDal; 
            this.user_Dal = userDal; 
        }

        /// <summary>
        /// Displays the admin dashboard.
        /// </summary>
        /// <returns>Returns the admin dashboard view.</returns>
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["ErrorMessage"] = "You must sign in first.";
                return RedirectToAction("SignIn", "Default");
            }

            return View();
        }

        /// <summary>
        /// Displays the list of all users.
        /// </summary>
        /// <returns>Returns the view with a list of users.</returns>
        public IActionResult ViewUsers()
        {
            List<Accounts> users = new List<Accounts>();

            try
            {
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    TempData["ErrorMessage"] = "You must sign in first.";
                    return RedirectToAction("SignIn", "Default");
                }

                users = account_Dal.GetAllUsers();
                if (users.Count == 0)
                {
                    TempData["ErrorMessage"] = "No users found.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while fetching users: " + ex.Message;
            }

            return View(users);
        }

        /// <summary>
        /// Displays the delete confirmation page for a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Returns the delete confirmation view with user details.</returns>
        public ActionResult Delete(int id)
        {
            var user = account_Dal.GetAllUsers().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ViewUsers");
            }

            return View(user); 
        }

        /// <summary>
        /// Handles the confirmed deletion of a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Redirects to the ViewUsers action.</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool isDeleted = account_Dal.DeleteUser(id);

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
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("ViewUsers");
            }
        }

        /// <summary>
        /// Displays the add new admin page.
        /// </summary>
        /// <returns>Returns the view to add a new admin.</returns>
        public IActionResult AddNewAdmin()
        {
            return View();
        }

        /// <summary>
        /// Handles the addition of a new admin account.
        /// </summary>
        /// <param name="admin">The admin account details.</param>
        /// <returns>Redirects to the sign-in page or returns the add admin view with errors.</returns>
        [HttpPost]
        public IActionResult AddNewAdmin(Accounts admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    admin.Password = EncryptionHelper.EncryptPassword(admin.Password);
                    admin.Role = "Admin";
                    account_Dal.AddNewAdmin(admin);
                    TempData["SuccessMessage"] = "Admin account created successfully.";
                    return RedirectToAction("SignIn", "Default");
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "Error while adding new admin: " + ex.Message;
            }

            return View(admin);
        }

        /// <summary>
        /// Displays the list of all applications.
        /// </summary>
        /// <returns>Returns the view with a list of applications.</returns>
        public IActionResult ApplicationList()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["ErrorMessage"] = "You must sign in first.";
                return RedirectToAction("SignIn", "Default");
            }

            List<Application> applications = new List<Application>();
            try
            {
                applications = user_Dal.GetAllApplications();
                if (applications.Count == 0)
                {
                    TempData["ErrorMessage"] = "No applications found.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while fetching the application list: " + ex.Message;
            }

            return View(applications);
        }

        /// <summary>
        /// Approves an application by updating its status to "Approved" in the database.
        /// Provides feedback to the user about the success or failure of the operation.
        /// Redirects the user to the ApplicationList view to refresh the list of applications.
        /// </summary>
        /// <param name="id">The ID of the application to be approved.</param>
        /// <returns>Redirects to the ApplicationList view after updating the application status.</returns>
        public IActionResult ApproveApplication(int id)
        {
            bool result = user_Dal.UpdateApplicationStatus(id, "Approved");
            if (result)
            {
                TempData["SuccessMessage"] = "Application approved successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to approve the application.";
            }
            return RedirectToAction("ApplicationList");
        }

        /// <summary>
        /// Rejects an application by updating its status to "Rejected" in the database.
        /// Provides feedback to the user about the success or failure of the operation.
        /// Redirects the user to the ApplicationList view to refresh the list of applications.
        /// </summary>
        /// <param name="id">The ID of the application to be rejected.</param>
        /// <returns>Redirects to the ApplicationList view after updating the application status.</returns>

        public IActionResult RejectApplication(int id)
        {
            bool result = user_Dal.UpdateApplicationStatus(id, "Rejected");
            if (result)
            {
                TempData["SuccessMessage"] = "Application rejected successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to reject the application.";
            }
            return RedirectToAction("ApplicationList");
        }


        /// <summary>
        /// Displays the delete confirmation page for an application.
        /// </summary>
        /// <param name="id">The ID of the application to delete.</param>
        /// <returns>Returns the delete confirmation view with application details.</returns>
        public IActionResult DeleteApplication(int id)
        {
            var application = user_Dal.GetApplicationById(id); 
            if (application == null)
            {
                TempData["ErrorMessage"] = "Application not found.";
                return RedirectToAction("AdminDashboard");
            }

            return View(application); 
        }

        /// <summary>
        /// Handles the confirmed deletion of an application.
        /// </summary>
        /// <param name="id">The ID of the application to delete.</param>
        /// <returns>Redirects to the ApplicationList action.</returns>
        [HttpPost, ActionName("DeleteApplication")]
        public IActionResult DeleteApplicationConfirmed(int id)
        {
            try
            {
                bool isDeleted = user_Dal.DeleteApplication(id);

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Application deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the application. It may not exist.";
                }

                return RedirectToAction("ApplicationList");
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("AdminDashboard");
            }
        }

        /// <summary>
        /// Displays the edit application form for the admin to update application details.
        /// </summary>
        /// <param name="id">The ID of the application to be edited.</param>
        /// <returns>The edit application view with the application details if found, otherwise redirects to the application list.</returns>
        public IActionResult AdminEditApplication(int id)
        {
            try
            {
                var application = user_Dal.GetApplicationById(id);

                if (application == null)
                {
                    TempData["ErrorMessage"] = "Application not found.";
                    return RedirectToAction("ApplicationList");
                }

                return View(application);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while fetching the application: " + ex.Message;
                return RedirectToAction("ApplicationList");
            }
        }

        /// <summary>
        /// Handles the submission of the admin edit application form and updates the application details.
        /// </summary>
        /// <param name="app">The application object with updated details.</param>
        /// <param name="Photo">The updated photo file (optional).</param>
        /// <param name="Document">The updated document file (optional).</param>
        /// <returns>Redirects to the admin dashboard on success or reloads the form with errors on failure.</returns>
        [HttpPost]
        public IActionResult AdminEditApplication(Application app, IFormFile Photo, IFormFile Document)
        {
            try
            {
                var existingApplication = user_Dal.GetApplicationById(app.AppID);
                if (existingApplication == null)
                {
                    TempData["ErrorMessage"] = "Application not found.";
                    return RedirectToAction("ApplicationList");
                }

                if (Photo != null && Photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Photo.CopyTo(memoryStream);
                        app.PhotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                if (Document != null && Document.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Document.CopyTo(memoryStream);
                        app.DocumentBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                ModelState.Remove("PhotoBase64");
                ModelState.Remove("DocumentBase64");

                if (ModelState.IsValid)
                {
                    user_Dal.UpdateApplication(app);
                    TempData["SuccessMessage"] = "Application updated successfully!";
                    return RedirectToAction("AdminDashboard");
                }

                TempData["ErrorMessage"] = "Failed to update application. Please check the form and try again.";
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while updating the application: " + ex.Message;
            }

            return View(app);
        }

        /// <summary>
        /// Displays a list of feedback submitted by users.
        /// </summary>
        /// <returns>Returns the view with the list of feedback.</returns>
        public IActionResult ViewFeedback()
        {
            List<Feedback> feedbackList = new List<Feedback>();

            try
            {
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    TempData["ErrorMessage"] = "You must sign in first.";
                    return RedirectToAction("SignIn", "Default");
                }

                feedbackList = account_Dal.GetAllFeedback(); 
                if (feedbackList.Count == 0)
                {
                    TempData["ErrorMessage"] = "No feedback available.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while fetching feedback: " + ex.Message;
            }

            return View(feedbackList); 
        }

        /// <summary>
        /// Handles the deletion of an feddback.
        /// </summary>
        /// <param name="feedId">The ID of the feedback to delete.</param>
        /// <returns>Redirects to the ViewApplication action.</returns>
        [HttpPost]
        public IActionResult DeleteFeedback(int feedId)
        {
            try
            {
                bool isDeleted = account_Dal.DeleteFeedback(feedId);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Feedback deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Feedback not found.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while deleting feedback: " + ex.Message;
            }
            return RedirectToAction("ViewFeedback");
        }

    }
}
