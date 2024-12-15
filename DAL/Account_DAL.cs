﻿using System.Data;
using Microsoft.Data.SqlClient;
using PassportGenerationSystem.Models;

namespace PassportGenerationSystem.DAL
{
    public class Account_DAL
    {
        private readonly string connectionString;

        public Account_DAL()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("PassportConnection");
        }

        //methhod for signup new user
        public void Signup(Accounts signup)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Signup";
                    cmd.Parameters.AddWithValue("@FirstName", signup.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", signup.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", signup.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", signup.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", signup.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", signup.Email);
                    cmd.Parameters.AddWithValue("@Address", signup.Address);
                    cmd.Parameters.AddWithValue("@State", signup.State);
                    cmd.Parameters.AddWithValue("@City", signup.City);
                    cmd.Parameters.AddWithValue("@Username", signup.Username);
                    cmd.Parameters.AddWithValue("@Password", signup.Password);
                    cmd.Parameters.AddWithValue("@Role", signup.Role);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //method for signin
        public Accounts Signin(string Username, string Password)
        {
            Accounts user = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_SignIn";
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new Accounts
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

            return user;
        }




        public List<Accounts> GetAllUsers()
        {
            List<Accounts> users = new List<Accounts>();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllUsers", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new Accounts
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Gender = reader["Gender"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString(),
                        State = reader["State"].ToString(),
                        City = reader["City"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }
            finally
            {
                conn.Close(); 
            }

            return users;
        }




        public bool DeleteUser(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteAccount", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }



        public void AddNewAdmin(Accounts admin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();

                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_AddNewAdmin";
                    cmd.Parameters.AddWithValue("@FirstName", admin.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", admin.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", admin.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", admin.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", admin.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", admin.Email);
                    cmd.Parameters.AddWithValue("@Address", admin.Address);
                    cmd.Parameters.AddWithValue("@State", admin.State);
                    cmd.Parameters.AddWithValue("@City", admin.City);
                    cmd.Parameters.AddWithValue("@Username", admin.Username);
                    cmd.Parameters.AddWithValue("@Password", admin.Password);
                    cmd.Parameters.AddWithValue("@Role", admin.Role); 

                    conn.Open();
                    cmd.ExecuteNonQuery(); 
                }
              
                finally
                {
                    conn.Close();
                }
            }
        }




    }
}