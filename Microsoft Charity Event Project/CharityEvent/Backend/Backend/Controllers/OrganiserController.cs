using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrganiserController : Controller
    {
        private readonly IConfiguration _configuration;
        public OrganiserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("OrganiserSignup")]
        public IActionResult Post(OrganiserModel organiser)
        {
            String databaseConnection = _configuration["DBConnection"];
            //Connecting to the database
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                string salt, hashedPassword, verificationCode;
                Hash.HashPassword(organiser.Password, out salt, out hashedPassword);
                verificationCode = Crypto.GenerateSalt();
                verificationCode = verificationCode.Replace(@"\", "");
                mySqlConnection.Open();

                MySqlCommand insertIntoAuthenticationCommand = mySqlConnection.CreateCommand();
                MySqlCommand insertIntoOrganiserCommand = mySqlConnection.CreateCommand();
                MySqlTransaction queryTransaction;
                queryTransaction = mySqlConnection.BeginTransaction();
                insertIntoAuthenticationCommand.Connection = mySqlConnection;
                insertIntoAuthenticationCommand.Transaction = queryTransaction;


                try
                {
                    //Inserting Email and Password into the authentication table
                    insertIntoAuthenticationCommand.CommandText = "insert into authentication(Email,Password,Salt)values(@Email,@Password,@Salt)";
                    insertIntoAuthenticationCommand.Parameters.AddWithValue("@Email", organiser.Email);
                    insertIntoAuthenticationCommand.Parameters.AddWithValue("@Password", hashedPassword);
                    insertIntoAuthenticationCommand.Parameters.AddWithValue("@Salt", salt);
                    insertIntoAuthenticationCommand.ExecuteNonQuery();
                    //Inserting Oraginser details into organiser table.

                    long accountId = insertIntoAuthenticationCommand.LastInsertedId;
                    insertIntoOrganiserCommand.CommandText = "insert into organiser(Email,Company_name,Company_description,Phone_number,Address,Zip,Created_date,VerificationCode,AccountID)values" +
                    "(@Email,@CompanyName,@Description,@PhoneNumber,@Address,@Zip,@CreatedAt,@VerificationCode,@AccountID)";

                    insertIntoOrganiserCommand.Parameters.AddWithValue("@Email", organiser.Email);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@CompanyName", organiser.CompanyName);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@Description", organiser.Description);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@PhoneNumber", organiser.PhoneNumber);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@Address", organiser.Address);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@Zip", organiser.Zip);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@VerificationCode", verificationCode);
                    DateTime CreatedAt = DateTime.Now;
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@CreatedAt", CreatedAt);
                    insertIntoOrganiserCommand.Parameters.AddWithValue("@AccountID", accountId);
                    insertIntoOrganiserCommand.ExecuteNonQuery();
                    queryTransaction.Commit();
                    SendEmail.verifyEmailMessage(organiser.Email, verificationCode, "Organiser");
                    return Ok("Succesfully Inserted");  //Status 200
                }
                catch (Exception e)
                {
                    try
                    {
                        queryTransaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (queryTransaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                            " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                    " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                    return BadRequest("101");    //Status 400
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
        }

        [HttpGet("OrganisersEvents/{email}")]
        public IActionResult getOrganisersEventList(String email)
        {
            List<EventModel> events = new List<EventModel>();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT count(te.ticket_id) AS NumberOfParticipants, e.Event_id, e.Email, e.Title, e.Type, e.Cost, " +
                                                             "e.Registration_begin, Registration_end, e.Image_path, e.Description, e.MaxNumberOfParticipants, e.PayoutSplitPercentageForWinner, " +
                                                             "e.CompetitionStarted, e.Invite_id, e.CompetitionCompleted FROM event e " +
                                                             "JOIN organiser o ON o.Email = e.email " +
                                                             "LEFT JOIN ticket_event te ON te.Event_id = e.Event_id WHERE e.Email = @email GROUP BY e.Event_id", con);
                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new EventModel
                    {
                        EventId = reader.GetInt32("Event_id"),
                        Email = reader["Email"].ToString(),
                        Title = reader["Title"].ToString(),
                        Type = reader["Type"].ToString(),
                        Cost = reader.GetDouble("Cost"),
                        Registration_begin = reader.GetDateTime("Registration_begin"),
                        Registration_end = reader.GetDateTime("Registration_end"),
                        Image_path = reader["Image_path"].ToString(),
                        Description = reader["Description"].ToString(),
                        NumberOfParticipants = reader.GetInt32("NumberOfParticipants"),
                        MaxNumberOfParticipants = reader.GetInt32("MaxNumberOfParticipants"),
                        PayoutSplitPercentageForWinner = reader.GetInt32("PayoutSplitPercentageForWinner"),
                        CompetitionStarted = reader.GetBoolean("CompetitionStarted"),
                        Invite_id = reader["Invite_id"].ToString(),
                        CompetitionCompleted = reader.GetBoolean("CompetitionCompleted")
                    });
                }

                if (!reader.HasRows)
                {
                    return BadRequest("Organiser has no events");
                }
                reader.Close();
            }
            return Ok(events);
        }

        [HttpGet("OrganisersDetails/{email}")]
        public IActionResult getOrganisersInfo(String email)
        {
            OrganiserModel organiser = new OrganiserModel();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT * FROM organiser WHERE Email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                reader.Read();

                organiser.Email = reader["Email"].ToString();
                organiser.CompanyName = reader["Company_name"].ToString();
                organiser.Description = reader["Company_Description"].ToString();
                organiser.PhoneNumber = reader["Phone_number"].ToString();
                organiser.Address = reader["Address"].ToString();
                organiser.Zip = reader["Zip"].ToString();
                organiser.Verified = reader.GetBoolean("Verified");

                reader.Close();
            }
            return Ok(organiser);
        }

        [HttpPost("OverifyEmail")]
        public IActionResult verifyEmail(VerificationModel verification)
        {
            String databaseConnection = _configuration["DBConnection"];

            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();
                //Retrieving Email and Password from database.
                MySqlCommand selectAuthenticationCommand = new MySqlCommand("SELECT VerificationCode FROM organiser WHERE email = @Email;", mySqlConnection);
                selectAuthenticationCommand.Parameters.AddWithValue("@Email", verification.Email);
                MySqlCommand verifyEmail = new MySqlCommand("UPDATE organiser SET verified = 1 WHERE email = @email", mySqlConnection);
                verifyEmail.Parameters.AddWithValue("@email", verification.Email);
                MySqlDataReader reader = selectAuthenticationCommand.ExecuteReader();

                reader.Read();
                string vcode = reader["VerificationCode"].ToString();
                reader.Close();
                if (vcode == verification.VerificationCode)
                {
                    verifyEmail.ExecuteNonQuery();
                    return Ok("vcode match!");
                }

            }
            return BadRequest("NO MATCH!");    //Status 400

        }

        [HttpGet("OsendEmailVerification/{email}")]
        public IActionResult sendEmailVerification(String email)
        {
            string verificationCode;
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT verificationCode FROM organiser WHERE email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                reader.Read();

                verificationCode = reader["verificationCode"].ToString();
                reader.Close();
            }
            SendEmail.verifyEmailMessage(email, verificationCode, "Organiser");
            return Ok("verification Code sent");
        }
    }
}
