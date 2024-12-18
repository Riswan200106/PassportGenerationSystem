using System.Data;
using Microsoft.Data.SqlClient;
using PassportGenerationSystem.Models;
using System.Configuration;

namespace PassportGenerationSystem.DAL
{
    public class User_DAL
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="User_DAL"/> class.
        /// Sets the connection string from the appsettings.json file.
        /// </summary>
        public User_DAL()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("PassportConnection");
        }

        /// <summary>
        /// Retrieves all applications from the database.
        /// </summary>
        /// <returns>A list of <see cref="Application"/> objects.</returns>
        public List<Application> GetAllApplications()
        {
            List<Application> applications = new List<Application>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetAllApplicationList", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var app = new Application
                            {
                                AppID = Convert.ToInt32(reader["AppID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = reader["Gender"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Nationality = reader["Nationality"].ToString(),
                                State = reader["State"].ToString(),
                                City = reader["City"].ToString(),
                                PhotoBase64 = reader["PhotoBase64"].ToString(),
                                DocumentBase64 = reader["DocumentBase64"].ToString(),
                                GuardianName = reader["GuardianName"].ToString(),
                                MaritalStatus = reader["MaritalStatus"].ToString(),
                                Status = reader["Status"].ToString(),
                                
                            };

                            applications.Add(app);
                        }

                        reader.Close();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return applications;
        }

        /// <summary>
        /// Creates a new application in the database.
        /// </summary>
        /// <param name="app">The application object containing the details to be created.</param>
        public void CreateApplication(Application app)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateApplication", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@FirstName", app.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", app.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", app.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", app.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", app.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", app.Email);
                    cmd.Parameters.AddWithValue("@Address", app.Address);
                    cmd.Parameters.AddWithValue("@Nationality", app.Nationality);
                    cmd.Parameters.AddWithValue("@State", app.State);
                    cmd.Parameters.AddWithValue("@City", app.City);
                    cmd.Parameters.AddWithValue("@PhotoBase64", app.PhotoBase64 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DocumentBase64", app.DocumentBase64 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GuardianName", app.GuardianName);
                    cmd.Parameters.AddWithValue("@MaritalStatus", app.MaritalStatus);
                    cmd.Parameters.AddWithValue("@Status", app.Status);
                    cmd.Parameters.AddWithValue("@UserId", app.UserId); // Pass UserId

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        /// <summary>
        /// Retrieves a specific application by its ID.
        /// </summary>
        /// <param name="id">The ID of the application.</param>
        /// <returns>An <see cref="Application"/> object if found; otherwise, null.</returns>
        public Application GetApplicationById(int id)
        {
            Application application = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ViewApplicationById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@AppID", id);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        application = new Application
                        {
                            AppID = Convert.ToInt32(reader["AppID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Gender = reader["Gender"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            Address = reader["Address"].ToString(),
                            Nationality = reader["Nationality"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                            PhotoBase64 = reader["PhotoBase64"].ToString(),
                            DocumentBase64 = reader["DocumentBase64"].ToString(),
                            GuardianName = reader["GuardianName"].ToString(),
                            MaritalStatus = reader["MaritalStatus"].ToString(),
                            Status = reader["Status"].ToString(),
                            UserId = Convert.ToInt32(reader["UserId"])
                        };
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            return application;
        }

        /// <summary>
        /// Updates an existing application in the database.
        /// </summary>
        /// <param name="app">The application object containing the updated details.</param>
        public void UpdateApplication(Application app)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateApplication", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@AppID", app.AppID);
                    cmd.Parameters.AddWithValue("@FirstName", app.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", app.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", app.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", app.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", app.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", app.Email);
                    cmd.Parameters.AddWithValue("@Address", app.Address);
                    cmd.Parameters.AddWithValue("@Nationality", app.Nationality);
                    cmd.Parameters.AddWithValue("@State", app.State);
                    cmd.Parameters.AddWithValue("@City", app.City);
                    cmd.Parameters.AddWithValue("@PhotoBase64", app.PhotoBase64 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DocumentBase64", app.DocumentBase64 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GuardianName", app.GuardianName);
                    cmd.Parameters.AddWithValue("@MaritalStatus", app.MaritalStatus);
                    cmd.Parameters.AddWithValue("@Status", app.Status);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        /// <summary>
        /// Deletes an application by its ID.
        /// </summary>
        /// <param name="appId">The ID of the application to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public bool DeleteApplication(int appId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteApplication", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AppID", appId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// Updates the status of an application in the database.
        /// </summary>
        /// <param name="applicationId">The ID of the application to update.</param>
        /// <param name="status">The new status to set (e.g., "Approved", "Rejected").</param>
        /// <returns>True if the update is successful, otherwise false.</returns>
        public bool UpdateApplicationStatus(int applicationId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateApplicationStatus", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@AppID", applicationId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of applications associated with a specific user by their UserId.
        /// This method executes a stored procedure to fetch application details from the database.
        /// The fetched data is then mapped into a list of <Application"> objects.
        /// </summary>
        /// <param name="userId">The ID of the user whose applications are to be retrieved.</param>
        /// <returns>A list of <see cref="Application"/> objects representing the applications submitted by the user.</returns>

        public List<Application> GetApplicationsByUserId(int userId)
        {
            List<Application> applications = new List<Application>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetApplicationsByUserId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var app = new Application
                                {
                                    AppID = Convert.ToInt32(reader["AppID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                    Gender = reader["Gender"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Nationality = reader["Nationality"].ToString(),
                                    State = reader["State"].ToString(),
                                    City = reader["City"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    PhotoBase64 = reader["PhotoBase64"].ToString(),
                                    DocumentBase64 = reader["DocumentBase64"].ToString(),
                                    GuardianName = reader["GuardianName"].ToString(),
                                    MaritalStatus = reader["MaritalStatus"].ToString()
                                };
                                applications.Add(app);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return applications;
        }
    }
}
