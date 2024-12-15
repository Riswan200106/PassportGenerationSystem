using System.Data;
using Microsoft.Data.SqlClient;
using PassportGenerationSystem.Models;
using System.Configuration;


namespace PassportGenerationSystem.DAL
{
    public class User_DAL
    {

        private readonly string connectionString;

        public User_DAL()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("PassportConnection");
        }

        public List<Application> GetAllApplications()
        {
            List<Application> applications = new List<Application>();

            using (SqlConnection connection = new SqlConnection(connectionString))
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
                                PhotoBase64= reader["PhotoBase64"].ToString(),
                                DocumentBase64 = reader["DocumentBase64"].ToString(),
                                GuardianName = reader["GuardianName"].ToString(),
                                MaritalStatus = reader["MaritalStatus"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                            };

                            applications.Add(app);
                        }

                        reader.Close();
                    }
            }

            return applications;
        }


        // Method to Create a New Application
        public void CreateApplication(Application app)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                cmd.Parameters.AddWithValue("@PhotoBase64", app.PhotoBase64);
                cmd.Parameters.AddWithValue("@DocumentBase64", app.DocumentBase64);
                cmd.Parameters.AddWithValue("@GuardianName", app.GuardianName);
                cmd.Parameters.AddWithValue("@MaritalStatus", app.MaritalStatus);
                cmd.Parameters.AddWithValue("@Username", app.Username);
                cmd.Parameters.AddWithValue("@Password", app.Password);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

       
        
    }
}
