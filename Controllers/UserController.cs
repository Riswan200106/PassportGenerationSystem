using Microsoft.AspNetCore.Mvc;
using PassportGenerationSystem.DAL;
using PassportGenerationSystem.Models;
using System.Text;

namespace PassportGenerationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly User_DAL user_Dal;

        /// <summary>
        /// Constructor to initialize UserController with a DAL instance using Dependency Injection.
        /// </summary>
        /// <param name="userDal">Instance of User_DAL provided by the DI container.</param>
        public UserController(User_DAL userDal)
        {
            this.user_Dal = userDal; 
        }

        /// <summary>
        /// Displays the user dashboard.
        /// </summary>
        /// <returns>View of the user dashboard.</returns>
        public IActionResult UserDashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            ViewData["Username"] = HttpContext.Session.GetString("Username");
            return View();
        }

        /// <summary>
        /// Displays the form to create a new application.
        /// </summary>
        /// <returns>View for creating an application.</returns>
        public IActionResult CreateApplication()
        {

            return View();
        }

        /// <summary>
        /// Handles the submission of the create application form.
        /// Also checking the UserId is matching that signin user is valid or not
        /// </summary>
        /// <param name="app">The application data submitted by the user.</param>
        /// <param name="Photo">The user's photo file.</param>
        /// <param name="Document">The user's document file.</param>
        /// <returns>Redirects to the user dashboard on success, otherwise reloads the form with errors.</returns>
        [HttpPost]
        public IActionResult CreateApplication(Application app, IFormFile Photo, IFormFile Document)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "User not logged in.";
                    return RedirectToAction("SignIn", "Default");
                }

                app.UserId = userId.Value; 

                ModelState.Remove("PhotoBase64");
                ModelState.Remove("DocumentBase64");

                if (ModelState.IsValid)
                {
                    if (Photo != null && Photo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Photo.CopyTo(memoryStream);
                            app.PhotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
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
                        return RedirectToAction("UserDashboard");
                    }
                }

                TempData["ErrorMessage"] = "Failed to create application. Please check the form and try again.";
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while creating the application: " + ex.Message;
            }

            return View(app);
        }

        /// <summary>
        /// Displays the form to edit an existing application.
        /// Before editing it checks the user is signin or not if not it will not allow editing.
        /// </summary>
        /// <param name="id">The ID of the application to edit.</param>
        /// <returns>View for editing the application or redirects to the application list if not found.</returns>
        public IActionResult EditApplication(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit an application.";
                    return RedirectToAction("SignIn", "Default");
                }

                var application = user_Dal.GetApplicationById(id);

                if (application == null || application.UserId != userId.Value)
                {
                    TempData["ErrorMessage"] = "Application not found or you do not have permission to edit it.";
                    return RedirectToAction("ViewMyApplications");
                }

                return View(application);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = "An error occurred while fetching the application: " + ex.Message;
                return RedirectToAction("ViewMyApplications");
            }
        }


        /// <summary>
        /// Handles the submission of the edit application form.
        /// </summary>
        /// <param name="app">The updated application data.</param>
        /// <param name="Photo">The updated photo file (optional).</param>
        /// <param name="Document">The updated document file (optional).</param>
        /// <returns>Redirects to the user dashboard on success, otherwise reloads the form with errors.</returns>
        [HttpPost]
        public IActionResult EditApplication(Application app, IFormFile Photo, IFormFile Document)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit an application.";
                    return RedirectToAction("SignIn", "Default");
                }

                var existingApplication = user_Dal.GetApplicationById(app.AppID);
                if (existingApplication == null || existingApplication.UserId != userId.Value)
                {
                    TempData["ErrorMessage"] = "You do not have permission to edit this application.";
                    return RedirectToAction("ViewMyApplications");
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
                    app.UserId = userId.Value;
                    user_Dal.UpdateApplication(app);
                    TempData["SuccessMessage"] = "Application updated successfully!";
                    return RedirectToAction("UserDashboard");
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
        /// Displays the details of a specific application.
        /// </summary>
        /// <param name="id">The ID of the application to view.</param>
        /// <returns>View displaying the application details or redirects to the application list if not found.</returns>
        public IActionResult DetailsApplication(int id)
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
                TempData["ErrorMessage"] = "An error occurred while fetching the application details: " + ex.Message;
                return RedirectToAction("ApplicationList");
            }
        }


        /// <summary>
        /// Generates and serves a downloadable passport document for an approved application.
        /// Fetches the application details by ID, validates that the application is approved,
        /// and generates a passport document in text format with relevant details.
        /// If successful, the passport is made available for download as a text file.
        /// If the application is not approved or not found, or if an error occurs, an appropriate message is displayed.
        /// </summary>
        /// <param name="id">The ID of the application for which the passport is generated.</param>
        /// <returns>A downloadable file containing the passport details if the application is approved; otherwise, a redirect to the dashboard with an error message.</returns>

        public IActionResult DownloadPassport(int id)
        {
            try
            {
                var application = user_Dal.GetApplicationById(id);

                if (application == null || application.Status != "Approved")
                {
                    return NotFound("Application not found or not approved.");
                }

                string passportContent = $"Passport\n\n" +
                                         $"Name: {application.FirstName} {application.LastName}\n" +
                                         $"Passport Number: PASS-{application.AppID:D6}\n" +
                                         $"Nationality: {application.Nationality}\n" +
                                         $"Date of Birth: {application.DateOfBirth:yyyy-MM-dd}\n" +
                                         $"Address: {application.Address}\n" +
                                         $"State: {application.State}\n" +
                                         $"City: {application.City}\n";

                byte[] fileBytes = Encoding.UTF8.GetBytes(passportContent);
                return File(fileBytes, "application/octet-stream", $"Passport_{application.AppID}.txt");
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = $"An error occurred while processing the request: {ex.Message}";
                return RedirectToAction("UserDashboard");
            }
        }

        /// <summary>
        /// Displays a list of all applications submitted by the logged-in user.
        /// </summary>
        /// <returns>A view displaying the list of user applications or an error message if applicable.</returns>
        public IActionResult ViewMyApplications()
        {
            List<Application> applications = new List<Application>();

            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "You must be logged in to view your applications.";
                    return RedirectToAction("SignIn", "Default"); 
                }
                applications = user_Dal.GetApplicationsByUserId(userId.Value);

                if (applications.Count == 0)
                {
                    TempData["ErrorMessage"] = "No applications found for your account.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                TempData["ErrorMessage"] = $"An error occurred while fetching your applications: {ex.Message}";
            }
            return View(applications);
        }
    }
}
