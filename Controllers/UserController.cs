using Microsoft.AspNetCore.Mvc;
using PassportGenerationSystem.DAL;
using PassportGenerationSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace PassportGenerationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly User_DAL user_Dal;

        public UserController()
        {
            user_Dal = new User_DAL();
        }

        public IActionResult UserDashboard()
        {
            return View();
        }

        // Create Application
        public IActionResult CreateApplication()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateApplication(Application app, IFormFile Photo, IFormFile Document)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Photo != null && Photo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Photo.CopyTo(memoryStream);
                            app.PhotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                            Console.WriteLine($"PhotoBase64: {app.PhotoBase64.Substring(0, 100)}..."); // Log or Debug
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Photo", "Photo is required.");
                    }

                    if (Document != null && Document.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Document.CopyTo(memoryStream);
                            app.DocumentBase64 = Convert.ToBase64String(memoryStream.ToArray());
                            Console.WriteLine($"DocumentBase64: {app.DocumentBase64.Substring(0, 100)}..."); // Log or Debug
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Document", "Document is required.");
                    }
                    if (ModelState.IsValid)
                    {
                        user_Dal.CreateApplication(app);
                        TempData["SuccessMessage"] = "Application created successfully!";
                        return RedirectToAction("ViewApplication");  //new { appId = app.AppID }
                    }
                }
                TempData["ErrorMessage"] = "Failed to create application. Please check the form and try again.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the application: " + ex.Message;
            }
            return View(app);
        }



        public IActionResult ApplicationList()
        {
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
                TempData["ErrorMessage"] = "An error occurred while fetching the application list: " + ex.Message;
            }

            return View(applications);
        }
    }
}
